# LoadTestToolbox
Lightweight tools for load testing web applications, written in C#

As with everything, these tools should be used only for good!

Intended to be run on a different machine than the one being tested (but ideally on the same LAN), these focus on server reponse time as the primary metric of performance.

## Setup

1. install the .NET Core runtime and Node.JS
1. follow [these instructions](https://github.com/Automattic/node-canvas#installation) to get Cairo + GTK installed for cross-platform image generation
1. run `./build.sh` (or `build.cmd` on Windows) to compile

## Hammer

Hammer helps measure load spikes by hammering on a specified site with a given range of simultaneous requests, and returns the average response time for each.

Given:

    ~$ ./hammer.sh http://example.com/ 1 100 chart.png
    
or on Windows:

    D:\LoadTestToolbox> hammer.cmd http://example.com/ 1 100 chart.png

hammer will make 1 simultaneous request, then 2, then 3, and so forth, up to 100.

Hammer is smart about orders of magnitude, so from 10 to 100, it will step up 10 requests at a time (10, 20, 30, etc.); step 100 for 100 to 1000 simultaneous requests (100, 200, ..., 1000); and so forth.

Hammer will give an output something like 

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

as it does its thing, and will then produce a [nice-ish](https://github.com/stevedesmond-ca/LoadTestToolbox/issues/1) chart upon completion.

## Drill

Drill helps measure long-term stability by constantly hitting a page at consistent intervals over a more extended period of time.

Given:

    ~$ ./drill.sh http://example.com/ 500 10 chart.png
    
or on Windows:

    D:\LoadTestToolbox> drill.cmd http://example.com/ 500 10 chart.png
    
drill will make 500 requests per second (at consistent 20ms intervals) for 10 seconds, each second outputting the average response time for the last second

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
    
and creating a similar chart upon completion.
