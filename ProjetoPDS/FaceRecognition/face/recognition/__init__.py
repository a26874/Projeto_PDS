import face_recognition
import cv2
import pickle

def location(image_location):
    # enviar localizações para a main
    face_location = face_recognition.face_locations(image_location, model="cnn")
    return face_location


def recognition(pathToFile):
    try:
        image_face = face_recognition.load_image_file(pathToFile)
        face_location = face_recognition.face_locations(image_face)
        face_encodings = face_recognition.face_encodings(image_face, face_location)
        face_encoding_binary = pickle.dumps(face_encodings)
        return face_encoding_binary
    except IndexError:
        print("I wasn't able to locate any faces in at least one of the images. Check the image files. Aborting...")
        quit()


def face_compare(face_location, imagecv2, imageface, loaded_list, resize):
    face_encodings = recognition(imageface, face_location)
    i = 0
    for top, right, bottom, left in face_location:
        top = int(top * resize)
        right = int(right * resize)
        bottom = int(bottom * resize)
        left = int(left * resize)

        results = face_recognition.compare_faces(loaded_list, face_encodings[i])

        exists = False
        for item in loaded_list:
            if (face_encodings[i] == item).all():
                exists = True
                break

        if not exists:
            loaded_list.append(face_encodings[i])

        imagecv2 = censure(results, imagecv2, left, top, right, bottom)
        i += 1
    return imagecv2


def censure(results, image, left, top, right, bottom):
    if any(results):
        cv2.rectangle(image, (left, top), (right, bottom), (0, 255, 0), 2)
    else:
        cv2.rectangle(image, (left, top), (right, bottom), (0, 0, 255), 2)
        face_image = image[top:bottom, left:right]
        face_image = cv2.GaussianBlur(face_image, (99, 99), 30)
        image[top:bottom, left:right] = face_image
    return image


def censure_click(image_number, face_location, image):
    top, right, bottom, left = face_location
    cv2.rectangle(image, (left, top), (right, bottom), (0, 0, 255), 2)
    face_image = image[top:bottom, left:right]
    face_image = cv2.GaussianBlur(face_image, (99, 99), 30)
    image[top:bottom, left:right] = face_image
    print("Blured face, press any key to exit blur mode.")
    cv2.imshow(f'Imagem {image_number}', image)


def checkMouseClick(posX, posY, image_number, image, resize, face_location):
    for face in face_location:
        top, right, bottom, left = face
        posX = posX / resize
        posY = posY / resize
        if left <= posX <= right and top <= posY <= bottom:
            print("Clicked on face")
            censure_click(image_number, face, image)
            break
