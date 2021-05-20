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
-e REDFIN__RESOURCES__0__RPID="69103754" \
-e REDFIN__RESOURCES__0__Slug="home" \
-e REDFIN__MQTT__BROKER="localhost" \
-e REDFIN__MQTT__DISCOVERYENABLED="true" \
mannkind/redfin2mqtt:latest
```

OR

```bash
REDFIN__RESOURCES__0__RPID="69103754" \
REDFIN__RESOURCES__0__Slug="home" \
REDFIN__MQTT__BROKER="localhost" \
REDFIN__MQTT__DISCOVERYENABLED="true" \
./redfin2mqtt 
```


## Configuration

Configuration happens via environmental variables

```bash
REDFIN__POLLINGINTERVAL                    - [OPTIONAL] The delay between zestimates lookups, defaults to "1.00:03:31"
REDFIN__RESOURCES__#__RPID                 - The n-th iteration of a Redfin Property ID for a specific property
REDFIN__RESOURCES__#__Slug                 - The n-th iteration of a slug to identify the specific Redfin Property ID
REDFIN__MQTT__TOPICPREFIX                  - [OPTIONAL] The MQTT topic on which to publish the collection lookup results, defaults to "home/redfin"
REDFIN__MQTT__DISCOVERYENABLED             - [OPTIONAL] The MQTT discovery flag for Home Assistant, defaults to false
REDFIN__MQTT__DISCOVERYPREFIX              - [OPTIONAL] The MQTT discovery prefix for Home Assistant, defaults to "homeassistant"
REDFIN__MQTT__DISCOVERYNAME                - [OPTIONAL] The MQTT discovery name for Home Assistant, defaults to "redfin"
REDFIN__MQTT__BROKER                       - [OPTIONAL] The MQTT broker, defaults to "test.mosquitto.org"
REDFIN__MQTT__USERNAME                     - [OPTIONAL] The MQTT username, default to ""
REDFIN__MQTT__PASSWORD                     - [OPTIONAL] The MQTT password, default to ""
```