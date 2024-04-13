from face import recognition
import face_recognition
import tkinter as tk
from tkinter import filedialog
import cv2
import numpy as np
import pickle

posX, posY = 0, 0


class ImageLoaderApp:
    """
    Classe que carrega as imagens
    """

    def __init__(self, master):
        self.master = master
        # listas com as imagens
        self.images_opencv = []
        self.images_face_recognition = []
        # botao para carregar imagens
        self.button_load = tk.Button(master, text="Load Images", command=self.load_images)
        self.button_load.pack()
        # botao para processar
        self.button_process = tk.Button(master, text="Process Images", command=self.process_images)
        self.button_process.pack()

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

    def mouse_callback(self, event, x, y, flags, param, image_number, image, resize_factor, face_locations):
        global posX, posY
        if event == cv2.EVENT_LBUTTONDOWN:
            face_clicked = 1
            posX, posY = x, y
            recognition.checkMouseClick(posX, posY, image_number, image, resize_factor, face_locations)

    def process_images(self):
        """
        Processa as imagens, ainda inacabada.
        :return:
        """
        i = 0
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

            image = recognition.face_compare(face_location, image_location, image_face, self.people, resize)
            cv2.imshow(f'Imagem {i}', image)
            cv2.waitKey(0)

            escolha = str(input("Are all faces recognized correctly?")).strip().upper()[0]
            if escolha == 'N':
                print("Click on the face to blur.")
                cv2.setMouseCallback(f'Imagem {i}',
                                     lambda event, x, y, flags, param, image_number=i, image=image_location,
                                            resize_factor=resize, face_locations=face_location: self.mouse_callback(
                                         event, x, y, flags, param, image_number, image, resize_factor, face_locations))
            cv2.waitKey(0)
            i += 1
        cv2.destroyAllWindows()
        self.images_opencv.clear()
        self.images_face_recognition.clear()
        with open('people.pkl', 'wb') as file:
            pickle.dump(self.people, file)


def main():
    root = tk.Tk()
    app = ImageLoaderApp(root)
    root.mainloop()


if __name__ == "__main__":
    main()
