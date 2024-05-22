import face_recognition
import cv2
import pickle
import json
import base64
import numpy as np
import os

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
        
def compareEncoding(encoding1, encoding2):
    try:
        decoded_encoding1 = base64.b64decode(encoding1)
        decoded_encoding2 = base64.b64decode(encoding2)

        face_encoding_1 = pickle.loads(decoded_encoding1)
        face_encoding_2 = pickle.loads(decoded_encoding2)

        if not isinstance(face_encoding_1, list):
            face_encoding_1 = [face_encoding_1]
        if not isinstance(face_encoding_2, list):
            face_encoding_2 = [face_encoding_2]
        results = face_recognition.compare_faces(face_encoding_1, face_encoding_2[0])
        return str(np.any(results))
    except IndexError:
        print('Erro.')


        
def small_image_face(fileName, pathToFile, left, top, right, bottom, utenteId):
    image = cv2.imread(pathToFile)
    if left > 0 and top > 0 and right > 0 and bottom > 0:
        face_image = image[top:bottom, left:right]
        try:
            currentDirectory = os.getcwd()
            folderName = "Fotos_Miniatura"
            newDirectory = os.path.join(currentDirectory, "website", "Imagens",folderName)
            if not os.path.exists(newDirectory):
                os.makedirs(newDirectory)
            auxUtenteId = str(utenteId)
            folderNameMin = "Fotos_Miniatura_"+fileName+auxUtenteId
            newFaceImageBlurred = os.path.join(newDirectory, folderNameMin + ".jpg")
            cv2.imwrite(newFaceImageBlurred, face_image)
            return newFaceImageBlurred
        except IndexError:
            print('Erro.')

def censure_results_utente(fileName, pathToFile, left, top, right, bottom):
    image = cv2.imread(pathToFile)
    if left > 0 and top > 0 and right > 0 and bottom > 0:
        face_image = image[top:bottom, left:right]
        face_image = cv2.GaussianBlur(face_image,(99,99),30)
        image[top:bottom, left:right] = face_image
        try:
            currentDirectory = os.getcwd()
            folderName = "Fotos_Desfocadas"
            newDirectory = os.path.join(currentDirectory,"Imagens","Fotos_Desfocadas")
            if not os.path.exists(newDirectory):
                os.makedirs(newDirectory)
            newImageBlurred = os.path.join(newDirectory, fileName + "Desfocada.jpg")
            cv2.imwrite(newImageBlurred, image)
            return newImageBlurred
        except IndexError:
            print('Erro')



def censure_results_identified(fileName, pathToFile, firstColor, secondColor, thirdColor, left, top, right, bottom):
    image = cv2.imread(pathToFile)
    cv2.rectangle(image, (left,top), (right,bottom),(firstColor, secondColor, thirdColor),2)
    try:
        currentDirectory = os.getcwd()
        folderName = "Fotos_Por_Verificar"
        newDirectory = os.path.join(currentDirectory, "website", "Imagens","Fotos_Por_Verificar")
        if not os.path.exists(newDirectory):
            os.makedirs(newDirectory)
        newImage = os.path.join(newDirectory, fileName + ".jpg")
        cv2.imwrite(newImage,image)
        return newImage
    except IndexError:
        print('teste')    
    

def censure_results_non_identified(fileName, pathToFile, firstColor, secondColor, thirdColor, left, top, right, bottom):
    image = cv2.imread(pathToFile)
    cv2.rectangle(image, (left,top), (right,bottom),(firstColor, secondColor, thirdColor),2)
    try:
        currentDirectory = os.getcwd()
        folderName = "Fotos_Por_Verificar"
        newDirectory = os.path.join(currentDirectory, "website", "Imagens","Fotos_Por_Verificar")
        if not os.path.exists(newDirectory):
            os.makedirs(newDirectory)
        newImage = os.path.join(newDirectory, fileName + "PorVerificar.jpg")
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

