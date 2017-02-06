REM Use -H localhost:2375 means connect to docker daemon running on localhost:2375
REM The default one listens on named pipe that Service Fabric shutdowns and start on localhost:2375 instead.
REM Run the commands below from a adming command prompt

set DOCKER_HOST=localhost:2375	

REM is docker running?
docker version 

REM build container images. Replace <myrepo> to be the name of your repo on https://hub.docker.com
docker build --tag=”<myrepo>/servicefabricfrontendservice:v1” --file=”FrontEndService.dockerfile” .
docker build --tag=”<myrepo>/servicefabricbackendservice:v1” --file=”BackEndService.dockerfile” .

REM published container images.Replace <myrepo> to be the name of your repo on https://hub.docker.com
docker login
docker push <myrepo>/servicefabricfrontendservice:11
docker push <myrepo>/servicefabricbackendservice:10

REM docker diagnostics
docker ps --no-trunc
