import sys
import os
import requests

def set_csrftoken(client):
    client.get(login_url)  # sets cookie
    if 'csrftoken' in client.cookies:
        # Django 1.6 and up
        return client.cookies['csrftoken']
    else:
        # older versions
        return client.cookies['csrf']


def save(client, data, url):
    r = client.post(url, data=data, headers=dict(Referer=url))

    print(r.status_code)



login_url = "http://127.0.0.1:8000/login/"
group_url = "http://127.0.0.1:8000/group-register/"

client = requests.session()

path = os.getcwd()
os.chdir('../Data/')
path = os.getcwd()

file = open("player_info.txt", "r")
player_info = file.readlines()
file.close()

csrftoken = set_csrftoken(client)

data = dict(
    name=player_info[4].replace('\n', ''),
    score=0,
    csrfmiddlewaretoken=csrftoken,
    next='/'
)

r = client.post(group_url, data=data, headers=dict(Referer=group_url))