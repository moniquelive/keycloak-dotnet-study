#!/bin/sh

curl -s\
  -d"grant_type=password&username=admin&password=12345678&client_id=admin-cli"\
  http://localhost:8080/realms/digital-dev/protocol/openid-connect/token # | jq '.access_token'
