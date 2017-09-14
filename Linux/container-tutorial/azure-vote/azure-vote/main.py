from flask import Flask, request, render_template
import os
import random
import redis
import socket
import sys

app = Flask(__name__)

# Load configurations
app.config.from_pyfile('config_file.cfg')
button1 =       app.config['VOTE1VALUE']  
button2 =       app.config['VOTE2VALUE']
title =         app.config['TITLE']

# Redis configurations
redis_server = os.environ['REDIS']

@app.route('/', methods=['GET', 'POST'])
def index():

    if request.method == 'GET':
        # Redis Connection
        try: 

            r = redis.StrictRedis(host=redis_server, port=6379, db=0)
            r.ping()

            # Init Redis
            if (r.exists(button1)):
                r.set(button1, r.get(button1))
            else:
                r.set(button1,0)

            if (r.exists(button2)):
                r.set(button2, r.get(button2))
            else: 
                r.set(button2, 0) 
        except redis.ConnectionError:
            exit('Failed to connect to Redis, terminating.')

        # Get current values
        vote1 = r.get(button1).decode('utf-8')
        vote2 = r.get(button2).decode('utf-8')            

        # Return index with values
        return render_template("index.html", value1=int(vote1), value2=int(vote2), button1=button1, button2=button2, title=title)

    elif request.method == 'POST':
        try: 
            r = redis.StrictRedis(host=redis_server, port=6379, db=0)
        except redis.ConnectionError:
            exit('Failed to connect to Redis, terminating.')
            
        if request.form['vote'] == 'reset':
            
            # Empty table and return results
            r.set(button1,0)
            r.set(button2,0)
            vote1 = r.get(button1).decode('utf-8')
            vote2 = r.get(button2).decode('utf-8')
            return render_template("index.html", value1=int(vote1), value2=int(vote2), button1=button1, button2=button2, title=title)
        
        else:

            # Insert vote result into DB
            vote = request.form['vote']
            r.incr(vote,1)
            
            # Get current values
            vote1 = r.get(button1).decode('utf-8')
            vote2 = r.get(button2).decode('utf-8')  
                
            # Return results
            return render_template("index.html", value1=int(vote1), value2=int(vote2), button1=button1, button2=button2, title=title)

if __name__ == "__main__":
    app.run()