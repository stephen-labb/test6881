import sys
import requests
import json
import re
import urllib.parse

orgranization = "vclimited"
# This file is aimed for generating alerts
method = "GET"
url = "https://vclimited.com/apis/token"
payload = None
header= {"Authorization": "Bearer tIyz6YMfwCiSWQVW7dt3hfOFzpVDQ1LVkV3PKYq4xm0AhXIjvh47BmYJ6oI47gqE", "Content-Type": "application/json"}

session = requests.session()
esponse = session.request(method=method,url=url,json=payload,verify=False)
response.raise_for_status()

# All right reserved by vclimited
