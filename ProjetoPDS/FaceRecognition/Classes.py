from pydantic import BaseModel

class Foto(BaseModel):
    path = ""
    fileName = ""
    left = ""
    top = ""
    right = ""
    bottom = ""    