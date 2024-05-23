import os
import os.path as op
from os import getcwd
from glob import glob
from datetime import datetime
import re
import json
from colorama import Fore
from pathlib import Path
import face_recognition
import recognition
from recognition import recognitionUsers, censure_results_utente
import uvicorn
import base64
import pickle
from typing import List
from fastapi import FastAPI, status, File, UploadFile, Form, HTTPException, Depends, Response
from fastapi.security import OAuth2PasswordBearer
from fastapi.responses import FileResponse, StreamingResponse
from fastapi.encoders import jsonable_encoder

from slowapi.errors import RateLimitExceeded
from slowapi import Limiter, _rate_limit_exceeded_handler
from slowapi.util import get_remote_address
from starlette.requests import Request

import Classes

limiter = Limiter(key_func=get_remote_address)
app = FastAPI(root_path='/server/api')  # for docs page behind proxy
app.state.limiter = limiter
app.add_exception_handler(RateLimitExceeded, _rate_limit_exceeded_handler)

oauth2_schema = OAuth2PasswordBearer(tokenUrl="token")


@app.get("/test/", status_code=status.HTTP_201_CREATED)
def hello_root():
    return {"name": 'name'}


@app.post('/BlurUsers/', status_code=status.HTTP_201_CREATED)
def Blur(infoFoto:Classes.Foto):
    left_value = int(infoFoto.left)
    top_value = int(infoFoto.top)
    right_value = int(infoFoto.right)
    bottom_value = int(infoFoto.bottom)
    return censure_results_utente(infoFoto.fileName, infoFoto.path, left_value, top_value, right_value, bottom_value)
        
if __name__ == '__main__':
    uvicorn.run('main:app', host='127.0.0.1', port=8000, reload=True)
