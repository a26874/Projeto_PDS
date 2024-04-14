import recognition
import face_recognition
import tkinter as tk
from tkinter import filedialog
import cv2
import numpy as np
import pickle

posX, posY = 0, 0


"""
Criar uma classe para uma pessoa, no qual contem o seu nome e uma lista de encodings
Possivelmente criar hashtable para performance, com as iniciais das pessoas
grupos de 3 ou 4
"""
class ImageLoaderApp:
    """
    Classe que carrega as imagens
    """

    def __init__(self, master):
        self.master = master
        # listas com as imagens
        self.images_opencv = []
        self.images_face_recognition = []
        # listas para cada imagem
        self.images_processed = []
        self.face_locations = []
        self.images_clicked = []
        """
        este dicionario serve pra saber quais encodings salvar,
        a chave será a localização da cara e o valor o seu encoding
        TEMOS UM PROBLEMA DE PERFORMANCE!!!! Ele guarda as localizações de todas as imagens
        Podia-se guardar noutro dicionario, no qual a chave é o numero da imagem
        e o valor é o dicionario com a localização e encoding
        """
        self.location_encoding = dict()
        # botao para carregar imagens
        self.button_load = tk.Button(master, text="Load Images", command=self.load_images)
        self.button_load.pack()
        # botao para processar
        self.button_process = tk.Button(master, text="Process Images", command=self.process_images)
        self.button_process.pack()
        # botao para mostrar as imagens
        self.button_show = tk.Button(master, text="Show Images", command=self.show_images)
        self.button_show.pack()
        with open('people.pkl', 'rb') as file:
            self.people = pickle.load(file)

    def load_images(self):
        """
       Função que carrega as imagens, tanto para opencv tanto para face_recognition
        :return:
        """
        file_paths = tk.filedialog.askopenfilenames(title="Select Images", filetypes=(
            ("Image files", "*.png;*.jpg;*.jpeg"), ("All files", "*.*")))
        for file_path in file_paths:
            try:
                image_face = face_recognition.load_image_file(file_path)

                with open(file_path, 'rb') as f:
                    image_data = np.asarray(bytearray(f.read()), dtype=np.uint8)
                image_location = cv2.imdecode(image_data, cv2.IMREAD_COLOR)

                if image_face is not None and image_location is not None:
                    self.images_face_recognition.append(image_face)
                    self.images_opencv.append(image_location)
                else:
                    print(f"Failed to load image: {file_path}")
            except Exception as e:
                print(f"Error loading image: {file_path} - {e}")

    def process_images(self):
        """
        Processa as imagens, identificando-as, censurando-as e deixa-las prontas para mostrar.
        :return:
        """
        if len(self.images_opencv) > 0:
            for image_location, image_face in zip(self.images_opencv, self.images_face_recognition):
                size = image_location.shape[:2]
                if size[0] > 1000 or size[1] > 1000:
                    if size[0] > size[1]:
                        resize = size[0] / 500
                    else:
                        resize = size[1] / 500
                else:
                    resize = 1
                image_resize = cv2.resize(image_location, (0, 0), fx=1 / resize, fy=1 / resize)
                face_location = recognition.location(image_resize)
                image_new = image_location.copy()
                self.face_locations.append(face_location)
                self.images_processed.append(
                    recognition.face_compare(face_location, image_new, image_face, self.people,
                                             self.location_encoding, resize))
            self.button_process.config(state=tk.DISABLED)
            self.button_load.config(state=tk.DISABLED)

    def mouse_censor(self, event, x, y, flags, param, image_number, image, resize_factor, face_loc_enc):
        """
        Recebe a localização de um click e censura nesse local se tiver uma cara
        :param event:
        :param x:
        :param y:
        :param flags:
        :param param:
        :param image_number:
        :param image:
        :param resize_factor:
        :param face_loc_enc:
        :return:
        """
        global posX, posY
        if event == cv2.EVENT_LBUTTONDOWN:
            posX, posY = x, y
            posX = posX / resize_factor
            posY = posY / resize_factor
            for location,bool in face_loc_enc.items():
                top, right, bottom, left = location
                if left <= posX <= right and top <= posY <= bottom and bool[1] != False:
                    if self.images_clicked:
                        self.images_clicked.pop(-1)
                    bool[1] = False
                    print("Clicked on face.\n Press any key to exit or continue to censor.")
                    self.images_clicked.append(recognition.censure_results([False], image, left, top, right, bottom))
                    cv2.imshow(f'Imagem {image_number}', self.images_clicked[-1])
                    break

    def mouse_uncensor(self, event, x, y, flags, param, image_number, image, resize_factor, face_loc_enc):
        """
        Recebe uma localização de uma cara e envia a imagem para censurar de novo
        :param event:
        :param x:
        :param y:
        :param flags:
        :param param:
        :param image_number:
        :param image:
        :param resize_factor:
        :param face_loc_enc:
        :return:
        """
        global posX, posY
        if event == cv2.EVENT_LBUTTONDOWN:
            posX, posY = x, y
            posX = posX / resize_factor
            posY = posY / resize_factor
            for location,bool in face_loc_enc.items():
                top, right, bottom, left = location
                if left <= posX <= right and top <= posY <= bottom and bool[1] != True:
                    if self.images_clicked:
                        self.images_clicked.pop(-1)
                    bool[1] = True
                    print("Clicked on face.\n Press any key to exit or continue to censor.")
                    self.images_clicked.append(recognition.censure_bool(image, face_loc_enc))
                    cv2.imshow(f'Imagem {image_number}', self.images_clicked[-1])
                    break
    def show_images(self):
        """
        Função que faz a apresentação das imagens. Aqui o utilizador irá realizar a
        visualização manual e corrigir os erros
        :return:
        """
        i = 0
        for image_processed, face_location,image_opencv in zip(self.images_processed, self.face_locations,self.images_opencv):
            cv2.imshow(f'Imagem {i}', image_processed)
            cv2.waitKey(1000)
            escolha = str(input("Are all faces recognized correctly?")).strip().upper()[0]
            image_new = image_opencv.copy()
            if escolha == 'N':
                size = image_processed.shape[:2]
                if size[0] > 1000 or size[1] > 1000:
                    if size[0] > size[1]:
                        resize = size[0] / 500
                    else:
                        resize = size[1] / 500
                else:
                    resize = 1
                print("Click on the face to blur. Press enter to continue.")
                cv2.setMouseCallback(f'Imagem {i}',
                                     lambda event, x, y, flags, param, image_number=i,
                                            image=image_processed,
                                            resize_factor=resize,
                                            face_loc_enc=self.location_encoding: self.mouse_censor(
                                         event, x, y, flags, param, image_number, image,
                                         resize_factor, face_loc_enc))
                while True:
                    key = cv2.waitKey(0)
                    if key != -1:
                        self.images_clicked.clear()
                        break
                print("Click on the face to unblur. Press enter to continue.")
                cv2.setMouseCallback(f'Imagem {i}',
                                     lambda event, x, y, flags, param, image_number=i,
                                            image=image_new,
                                            resize_factor=resize,
                                            face_loc_enc=self.location_encoding: self.mouse_uncensor(
                                         event, x, y, flags, param, image_number, image,
                                         resize_factor, face_loc_enc))
                while True:
                    key = cv2.waitKey(0)
                    if key != -1:
                        break
                cv2.destroyAllWindows()

        self.button_process.config(state=tk.NORMAL)
        self.button_load.config(state=tk.NORMAL)
        i = 0

        for image in self.images_opencv:
            nome_imagem = str(input("input the image name: ")).strip()
            cv2.imwrite(f"C:/Users/user/PycharmProjects/AutoPhotoSorter/Novas Images/{nome_imagem}.png", recognition.censure_bool(image, self.location_encoding, False))
            i += 1
        for value in self.location_encoding:
            if value[1] == True:
                exists = False
                for item in self.people:
                    if (value[0] == item).all():
                        exists = True
                        break
                if not exists:
                    self.people.append(value)
        self.location_encoding.clear()
        with open('people.pkl', 'wb') as file:
            pickle.dump(self.people, file)
        self.images_processed.clear()
        self.images_opencv.clear()
        self.images_face_recognition.clear()
def main():
    root = tk.Tk()
    app = ImageLoaderApp(root)
    root.mainloop()


if __name__ == "__main__":
    main()
