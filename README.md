# LoadTestToolbox
Lightweight tools for load testing web applications, written in C#

As with everything, these tools should be used only for good!

Obviously intended to be run on a different machine than the one being tested.

## Hammer

Hammers on a specified site with a given range of simultaneous requests, and returns the average response time for each. Given:

    hammer http://example.com/ 1 100 chart.png

Hammer will make 1 simultaneous request, then 2, then 3, and so forth, up to 100.

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
