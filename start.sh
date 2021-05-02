#! /bin/bash
docker-compose build && docker-compose up -d && docker logs redis-playground-api
