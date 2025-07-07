#!/bin/sh

FILE=$1
TOKEN=$2

curl -d @${FILE} -s\
  -H "Authorization: Bearer ${TOKEN}"\
  -H "content-type: application/json"\
  http://localhost:8080/admin/realms/digital-dev/users
