import face_recognition
import cv2
import pickle
import json
import base64
import numpy as np
import os
def location(image_location):
    # enviar localizações para a main
    face_location = face_recognition.face_locations(image_location, model="cnn")
    return face_location


def recognition(pathToFile):
    encodingList = {}
    try:
        image_face = face_recognition.load_image_file(pathToFile)
        face_locations = face_recognition.face_locations(image_face)
        for face in face_locations:
            face_encoding = face_recognition.face_encodings(image_face,[face])
            face_encoding_binary = base64.b64encode(pickle.dumps(face_encoding)).decode("utf-8")
            encodingList[tuple(face)] = face_encoding_binary[:]
        return encodingList
    except IndexError:
        print("I wasn't able to locate any faces in at least one of the images. Check the image files. Aborting...")
        quit()

def singleRecognition(pathToFile):
    try:
        image_face = face_recognition.load_image_file(pathToFile)
        face_location = face_recognition.face_locations(image_face)
        face_encoding = face_recognition.face_encodings(image_face, face_location)
        face_encoding_binary = pickle.dumps(face_encoding)
        
        return face_encoding_binary
    except IndexError:
        print("asd")
        
        
def compareEncoding(encoding1, encoding2):
    try:
        face_encoding_1 = pickle.loads(encoding1)
        face_encoding_2 = pickle.loads(encoding2)
        
        if not isinstance(face_encoding_1, list):
            face_encoding_1 = [face_encoding_1]
        if not isinstance(face_encoding_2, list):
            face_encoding_2 = [face_encoding_2]
        results = face_recognition.compare_faces(face_encoding_1, face_encoding_2)
        return str(np.any(results))
    except IndexError:
        print('asd')


def face_compare(face_location, imagecv2, imageface, people_list,images_face_encodings, resize):
    face_encodings = recognition(imageface, face_location)
    i = 0
    for location,encoding in zip(face_location,face_encodings):
        images_face_encodings[tuple(location)] = []
        images_face_encodings[tuple(location)].append(encoding)
    for top, right, bottom, left in face_location:
        key = (top, right, bottom, left)
        top = int(top * resize)
        right = int(right * resize)
        bottom = int(bottom * resize)
        left = int(left * resize)

        results = face_recognition.compare_faces(people_list, face_encodings[i])
        if any(results):
            images_face_encodings[key].append(True)
        else:
            images_face_encodings[key].append(False)
        imagecv2 = censure_results(results, imagecv2, left, top, right, bottom)
        i += 1
    return imagecv2


def censure_results(results, image, left, top, right, bottom):
    if any(results):
        cv2.rectangle(image, (left, top), (right, bottom), (0, 255, 0), 2)
    else:
        cv2.rectangle(image, (left, top), (right, bottom), (0, 0, 255), 2)
        face_image = image[top:bottom, left:right]
        face_image = cv2.GaussianBlur(face_image, (99, 99), 30)
        image[top:bottom, left:right] = face_image
    return image


def add_utente_click (fileName, pathToFile, posX, posY, left, top, right, bottom):
    image = cv2.imread(pathToFile)
    if left <= posX <= right and top <= posY <= bottom:
        return True
    return False

def censure_results_click(fileName, pathToFile, posX, posY, left, top, right, bottom):
    image = cv2.imread(pathToFile)
    if left <= posX <= right and top <= posY <= bottom:
        face_image = image[top:bottom, left:right]
        face_image = cv2.GaussianBlur(face_image,(99,99),30)
        image[top:bottom, left:right] = face_image
        try:
            currentDirectory = os.getcwd()
            folderName = "Fotos_Desfocadas"
            newDirectory = os.path.join(currentDirectory, folderName)
            if not os.path.exists(folderName):
                os.makedirs(newDirectory)
            newImageBlurred = os.path.join(newDirectory, fileName+ "FotoDesfocada.jpg")
            cv2.imwrite(newImageBlurred, image)
            return newImageBlurred
        except IndexError:
            print('teste')


def censure_results_2(fileName, pathToFile, left, top, right, bottom):
    image = cv2.imread(pathToFile)
    cv2.rectangle(image, (left,top), (right,bottom),(0,0,255),2)
    face_image = image[top:bottom, left:right]
    image[top:bottom, left:right] = face_image
    try:
        currentDirectory = os.getcwd()
        folderName = "Fotos_Por_Verificar"
        newDirectory = os.path.join(currentDirectory, folderName)
        if not os.path.exists(folderName):
            os.makedirs(newDirectory)
        newImage = os.path.join(newDirectory, fileName +"PorVerificar.jpg")
        cv2.imwrite(newImage,image)
        return newImage
    except IndexError:
        print('teste')    
    
    

def censure_bool(image,face_loc_enc,border = True):
    """
    Função que recebe uma imagem, um dicionario de cada localização,encodings e se deve identificar e
    tambem, se deve fazer a caixa verde ou vermelha, este border serve para depois de tudo confirmado
    para censurar sem bordas.
    :param image:
    :param face_loc_enc:
    :param border:
    :return:
    """
    for location,bool in face_loc_enc.items():
        top, right, bottom, left = location
        if bool[1]:
            if border: cv2.rectangle(image, (left, top), (right, bottom), (0, 255, 0), 2)
        else:
            if border: cv2.rectangle(image, (left, top), (right, bottom), (0, 0, 255), 2)
            face_image = image[top:bottom, left:right]
            face_image = cv2.GaussianBlur(face_image, (99, 99), 30)
            image[top:bottom, left:right] = face_image
    return image

