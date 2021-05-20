# redfin2mqtt

[![Software
License](https://img.shields.io/badge/License-MIT-orange.svg?style=flat-square)](https://github.com/mannkind/redfin2mqtt/blob/main/LICENSE.md)
[![Build Status](https://github.com/mannkind/redfin2mqtt/workflows/Main%20Workflow/badge.svg)](https://github.com/mannkind/redfin2mqtt/actions)
[![Coverage Status](https://img.shields.io/codecov/c/github/mannkind/redfin2mqtt/main.svg)](http://codecov.io/github/mannkind/redfin2mqtt?branch=main)

An experiment to publish Redfin estimates to MQTT.
!!! The Redfin API is super unofficial, this will 100% break !!!


## Use

The application can be locally built using `dotnet build` or you can utilize the multi-architecture Docker image(s).

### Example

```bash
docker run \
-e REDFIND__RESOURCES__0__RPID="69103754" \
-e REDFIND__RESOURCES__0__Slug="home" \
-e REDFIND__MQTT__BROKER="localhost" \
-e REDFIND__MQTT__DISCOVERYENABLED="true" \
mannkind/redfin2mqtt:latest
```

OR

```bash
REDFIND__RESOURCES__0__RPID="69103754" \
REDFIND__RESOURCES__0__Slug="home" \
REDFIND__MQTT__BROKER="localhost" \
REDFIND__MQTT__DISCOVERYENABLED="true" \
./redfin2mqtt 
```


## Configuration

Configuration happens via environmental variables

```bash
REDFIND__POLLINGINTERVAL                    - [OPTIONAL] The delay between zestimates lookups, defaults to "1.00:03:31"
REDFIND__RESOURCES__#__RPID                 - The n-th iteration of a Redfin Property ID for a specific property
REDFIND__RESOURCES__#__Slug                 - The n-th iteration of a slug to identify the specific Redfin Property ID
REDFIND__MQTT__TOPICPREFIX                  - [OPTIONAL] The MQTT topic on which to publish the collection lookup results, defaults to "home/redfin"
REDFIND__MQTT__DISCOVERYENABLED             - [OPTIONAL] The MQTT discovery flag for Home Assistant, defaults to false
REDFIND__MQTT__DISCOVERYPREFIX              - [OPTIONAL] The MQTT discovery prefix for Home Assistant, defaults to "homeassistant"
REDFIND__MQTT__DISCOVERYNAME                - [OPTIONAL] The MQTT discovery name for Home Assistant, defaults to "redfin"
REDFIND__MQTT__BROKER                       - [OPTIONAL] The MQTT broker, defaults to "test.mosquitto.org"
REDFIND__MQTT__USERNAME                     - [OPTIONAL] The MQTT username, default to ""
REDFIND__MQTT__PASSWORD                     - [OPTIONAL] The MQTT password, default to ""
```

## Prior Implementations

### Golang
* Last Commit: [682c80313cee963bd1c6c0948577ebffd9d551d2](https://github.com/mannkind/redfin2mqtt/commit/682c80313cee963bd1c6c0948577ebffd9d551d2)
* Last Docker Image: [mannkind/redfin2mqtt:v0.4.20061.0152](https://hub.docker.com/layers/mannkind/redfin2mqtt/v0.4.20061.0152/images/sha256-4c450faf8bbac5a6dd55fdb084cebdeae256c01a9b27580b9f0302ec98e6842c?context=explore)