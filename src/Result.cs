using System.Net;

namespace LoadTestToolbox;

public record struct Result(int ResponseCode, double Duration);
