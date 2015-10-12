# LoadTestToolbox
Lightweight tools for load testing web applications, written in C#

As with everything, these tools should be used only for good!

## Hammer

Hammers on a specified site with a given range of simultaneous requests, and returns the average response time for each

    hammer http://example.com/ 1 100

will give an output something like 

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
