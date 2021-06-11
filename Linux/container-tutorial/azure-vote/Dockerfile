FROM    tiangolo/uwsgi-nginx-flask:latest

RUN     pip install redis
 
ADD     /azure-vote /app

WORKDIR /app

ENV REDIS redisbackend.testapp

EXPOSE 80
