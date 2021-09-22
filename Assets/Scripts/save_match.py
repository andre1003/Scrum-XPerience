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
match_url = "http://127.0.0.1:8000/match-register/"
group_url = "http://127.0.0.1:8000/group-register/"
decision_url = "http://127.0.0.1:8000/decision-register/"

client = requests.session()

csrftoken = set_csrftoken(client)

data = dict(username=sys.argv[1], password=sys.argv[2], csrfmiddlewaretoken=csrftoken, next='/')
r = client.post(login_url, data=data, headers=dict(Referer=login_url))

# I need to reset csrftoken because login redirects to home, ant csrftoken is cleared
csrftoken = set_csrftoken(client)

path = os.getcwd()
os.chdir('../Data/')
path = os.getcwd()

file = open("player_info.txt", "r")
player_info = file.readlines()
file.close()

data = dict(
    role=player_info[0].replace('\n', ''),
    hits=int(player_info[1].replace('\n', '')),
    mistakes=int(player_info[2].replace('\n', '')),
    individual_feedback=player_info[3].replace('\n', ''),
    group=player_info[4].replace('\n', ''),
    csrfmiddlewaretoken=csrftoken,
    next='/'
)

# Not working. It doesn't send HttpResponse
r = client.post(match_url, data=data, headers=dict(Referer=match_url))

print(f'{r.status_code}')

match_id = r.json()['match_id']



file = open("path.txt", "r")
decisions_path = file.readline()
file.close()

decision_url += match_id + "/"



########### Save decision
os.chdir(decisions_path)
files = os.listdir()

for f in files:
    file = open(f, 'r')
    decision = file.readlines()
    file.close()

    is_mistake = False
    if decision[2] == 'True':
        is_mistake = True,
        
    data = dict(
        decision=decision[0].replace('\n', ''),
        scenery=decision[1].replace('\n', ''),
        is_mistake=is_mistake,
        csrfmiddlewaretoken=csrftoken,
        next='/'
    )

    save(client, data, decision_url)