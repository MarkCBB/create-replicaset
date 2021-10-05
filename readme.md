# Introduction

This project creates a replica set in a MongoDB instance. Its DNS name must be mongo1.

# Get started

## Make sure that:

* A MongoDB instance is up and running and reachable under the DNS mongo1.
* The MongoDB instance running under replica set configuration set in the config file or in the arguments that run the mongod process ([More info](https://docs.mongodb.com/manual/tutorial/deploy-replica-set/)).
* Dotnet core 5 is installed in your laptop.
* If you wish to build an image, Docker must be installed and its service running.

## To run it:

```
dotnet run
```

## To build an image:

```
dotnet publish -c Release
```

```
docker build -t <image_name> -f dockerfile .
```

## If you want to publish the image

```
docker image push <image_name>
```
