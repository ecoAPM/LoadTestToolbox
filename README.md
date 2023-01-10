# LoadTestToolbox

Lightweight tools for load testing web applications, written in C#

[![NuGet version](https://img.shields.io/nuget/v/LoadTestToolbox?logo=nuget&label=Install)](https://nuget.org/packages/LoadTestToolbox)
[![CI](https://github.com/ecoAPM/LoadTestToolbox/actions/workflows/CI.yml/badge.svg)](https://github.com/ecoAPM/LoadTestToolbox/actions/workflows/CI.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_LoadTestToolbox&metric=coverage)](https://sonarcloud.io/dashboard?id=ecoAPM_LoadTestToolbox)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_LoadTestToolbox&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_LoadTestToolbox)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_LoadTestToolbox&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_LoadTestToolbox)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_LoadTestToolbox&metric=security_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_LoadTestToolbox)

**These tools should only be used on applications that you own or have permission to test against**

## Installation

1. Download the latest release for your operating system and architecture
1. Extract the archive to the directory of your choosing

### Install as a .NET Global Tool

`~$ dotnet tool install --global LoadTestToolbox` (requires .NET 6 SDK)

## Usage

For the most accurate results, run LoadTestToolbox on a different machine (ideally on the same LAN as) than the one being tested.

`~$ ltt <tool> [options]`

## Tools

LoadTestToolbox currently consists of three tools:
- `drill`
- `hammer`
- `nailgun`

### Drill

Drill helps measure long-term stability by constantly requesting a page at consistent intervals over a more extended period of time.

Example:

   `~$ ltt drill --url http://192.168.1.100/ --rps 500 --duration 10 --filename chart.png`

In the above command, LoadTestToolbox will make 500 requests per second (at consistent 20ms intervals) for 10 seconds.

Upon completion, a Cartesian chart is output to the specified file, showing each request and its response time.

### Hammer

Hammer helps measure load spikes by "hammering" a specified URL with a given range of simultaneous requests, and returns the average response time for each.

Example:

`~$ ltt hammer --url http://192.168.1.100/ --min 1 --max 100 --filename chart.png`

The above command will make 1 simultaneous request, then 2, then 3, and so forth, up to 100.

> The hammer command is smart about orders of magnitude, so from 10 to 100, it will step up 10 requests at a time (10, 20, 30, etc.); step 100 for 100 to 1000 simultaneous requests (100, 200, ..., 1000); and so forth.

A Cartesian chart showing the average response time for each set of requests is output to the specified file upon completion.

### Nailgun

Nailgun shows how a specified URL handles a single large influx of requests.

`~$ ltt nailgun --url http://192.168.1.100/ --requests 1000 --filename chart.png`

The above command sends 1000 requests all at once, and records the response times for each.

A Cartesian chart showing how the URL responded to each of the requests is output to the specified file upon completion.

## Options

### Required for all requests

- `-u`/`--url`: the URL to send to
- `-f`/`--filename`: the file to output the chart to

### Optional for any request

- `-m`/`--method`: the HTTP method to send (default: `GET`)
- `-H`/`--header`: the HTTP header(s) to send (default: none)
- `-b`/`--body`: the HTTP body to send (default: none)

### Required for `drill`

- `-r`/`--rps`: the number of requests per second to send
- `-d`/`--duration`: the number of seconds to send requests for

### Required for `hammer`

- `--min`: the minimum number of requests to send
- `--max`: the maximum number of requests to send

### Required for `nailgun`

- `-r`/`--requests` the number of requests to send

## Contributing

Please be sure to read and follow ecoAPM's [Contribution Guidelines](CONTRIBUTING.md) when submitting issues or pull requests.
