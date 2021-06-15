FROM    tiangolo/uwsgi-nginx-flask:python3.6

RUN     pip install redis
 
ADD     /azure-vote /app

WORKDIR /app

ENV REDIS redisbackend.testapp

EXPOSE 80
