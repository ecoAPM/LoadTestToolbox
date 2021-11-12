# LoadTestToolbox

Lightweight tools for load testing web applications, written in C#

[![NuGet version](https://img.shields.io/nuget/v/LoadTestToolbox?logo=nuget&label=Install)](https://nuget.org/packages/LoadTestToolbox)
[![CI](https://github.com/ecoAPM/LoadTestToolbox/actions/workflows/CI.yml/badge.svg)](https://github.com/ecoAPM/LoadTestToolbox/actions/workflows/CI.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_LoadTestToolbox&metric=coverage)](https://sonarcloud.io/dashboard?id=ecoAPM_LoadTestToolbox)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_LoadTestToolbox&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_LoadTestToolbox)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_LoadTestToolbox&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_LoadTestToolbox)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_LoadTestToolbox&metric=security_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_LoadTestToolbox)

**These tools should only be used on applications that you own or have permission to test against**

## Requirements

- .NET SDK 6.0

Linux:
- `libgdiplus`

## Installation

`~$ dotnet tool install --global LoadTestToolbox`

## Usage

For the most accurate results, run LoadTestToolbox on a different machine (ideally on the same LAN as) than the one being tested.

`~$ ltt <tool> [options]`

## Tools

LoadTestToolbox currently consists of two tools:
- `drill`
- `hammer`

### Drill

Drill helps measure long-term stability by constantly requesting a page at consistent intervals over a more extended period of time.

Example:

   `~$ ltt drill -url http://example.com/ --rps 500 --duration 10 --filename chart.png`

In the above command, LoadTestToolbox will make 500 requests per second (at consistent 20ms intervals) for 10 seconds. After each second, the average response time for the last second is output to the console:
```
1: 77.91 ms
2: 51.02 ms
3: 9.37 ms
4: 11.78 ms
5: 9.76 ms
6: 10.61 ms
7: 115.06 ms
8: 126.69 ms
9: 96.69 ms
10: 71.15 ms
```

Upon completion, a Cartesian chart is output to the specified file, showing each request and its response time.

### Hammer

Hammer helps measure load spikes by "hammering" a specified URL with a given range of simultaneous requests, and returns the average response time for each.

Example:

`~$ ltt hammer --url http://example.com/ --min 1 --max 100 --filename chart.png`

The above command will make 1 simultaneous request, then 2, then 3, and so forth, up to 100.

> The hammer command is smart about orders of magnitude, so from 10 to 100, it will step up 10 requests at a time (10, 20, 30, etc.); step 100 for 100 to 1000 simultaneous requests (100, 200, ..., 1000); and so forth.

The average response time for each set of requests is output to the console:
```
1: 31.24 ms
2: 7.81 ms
3: 5.21 ms
4: 3.9 ms
5: 6.25 ms
6: 5.21 ms
7: 2.23 ms
8: 1.95 ms
9: 3.48 ms
10: 4.69 ms
20: 10.93 ms
30: 13.01 ms
40: 30.85 ms
50: 15.62 ms
60: 19 ms
70: 54.45 ms
80: 58.38 ms
90: 71.7 ms
100: 58.12 ms
```

A Cartesian chart with the above data is output to the specified file upon completion.

## Contributing

Please be sure to read and follow ecoAPM's [Contribution Guidelines](CONTRIBUTING.md) when submitting issues or pull requests.