using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Security.Cryptography.X509Certificates;

namespace CybsClass.Cybersource.PosTransactions
{
    public static class WssPosTransaction
    {
        public static async Task<string> WssCybsTransaction(string request)
        {
            using (var ws = new ClientWebSocket())
            {
                
                // 1. Load certificate and private key (from PEM strings in this example)
                //string certPem = @"-----BEGIN RSA PRIVATE KEY-----MIIEoDCCA4igAwIBAgIRAI4AMByZ5LrDIfSwRWZ1/eMwDQYJKoZIhvcNAQELBQAwfzELMAkGA1UEBhMCVVMxEjAQBgNVBAoMCVZpc2EgSW5jLjEvMC0GA1UECwwmVmlzYSBJbnRlcm5hdGlvbmFsIFNlcnZpY2UgQXNzb2NpYXRpb24xKzApBgNVBAMMIlZp\r\nc2EgTm9uLVByb2QgVVMtRWFzdC0xIElzc3VpbmcgQ0EwHhcNMjUwMzA1MjIwNTU0WhcNMjcwNTI0MjMwNTU0WjCBjzE0MDIGA1UEAwwrWllYMTIzLmNlZjY0NjRhLTI0YjEtNGQ5ZS04NTU5LThjYjY0NzM0MTlhNjEMMAoGA1UECwwDTUFQMQ0wCwYDVQQKDARWaXNhMQ8wDQYDVQQHDAZNdW5pY2gxCzAJBgNVBAgMAkJZMQswCQYDVQQGEwJERTEPMA0GA1UEBRMGWllYMTIzMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwWg/QgMYRf39SgOKeP30m9WFndYzzvwevmOQt04RceAv9nUsM87kdcwVhxU1pJPJlEHWv/PE1ePU/d005IxYfyBDhxEC3bVRrS4imlx7TcBEtC7DyeNzwOSOXwJAGK4kFUCfC7RYNIC/zaNIBtLxgTgcIt/3I53T0eJEhkTDxfyutHr4izv2I2n61S04\r\nmU2Rp0+BR7w18QlXStTzoXimHuMnksmgtCf8XN7l6LHZnHYDlFeYi/TEsjdSqJst\r\nCUoIs/voKtZZNnXlKXETz8jr8ncU2yKNDsaaEge7s1x3gmOj01O0uG+qwT8Ux241\r\nkg6WFZi0dCgNOFSfXDgdWU+LqwIDAQABo4IBBDCCAQAwIAYDVR0RBBkwF4IVWllY\r\nMTIzLmN5YnMuc2VjbGliLmlvMAkGA1UdEwQCMAAwHwYDVR0jBBgwFoAU/32xdaWY\r\n66hsSWedTxYtmPGipPwwHQYDVR0OBBYEFODZxJU7r68BD+e5oQW23frX5brmMA4G\r\nA1UdDwEB/wQEAwIFoDAdBgNVHSUEFjAUBggrBgEFBQcDAQYIKwYBBQUHAwIwYgYD\r\nVR0fBFswWTBXoFWgU4ZRaHR0cDovL2QxMDZhbHZlN2toaDNlLmNsb3VkZnJvbnQu\r\nbmV0L2NybC8yOTU0YzRhOS0yN2MyLTQwZjMtYWMxOC04YTUyY2QyOTU5ZDYuY3Js\r\nMA0GCSqGSIb3DQEBCwUAA4IBAQCXEygF2gjAPEErDwS2IbHAXW1xXxtsXd3g/ovh\r\ndirmLL1gJV7a4jiwLkAzeBghrDiq+oRS66mBrM+T5uYdeL1FnMI65RODIjzRrKIV0L0/MYmJHyvczVo2hF6DdlGSCMxyawKwrVY5mBJmCP2IhHZfca/XnhGyiT0psGeosVx4+SU9lFYCyn0dKndRBib23iqftckmbF7T4SURxb9DhYeL+NZq2NVfo6YtMiM6UOQVgxUKhLz3oKR1PjSlAJ7d1smU+37Zf2D0nx5ty76TDGEe/Ao2x78eQlgLUT5yr4fPxYiNa+nxS7PTml2NbdNig9z4QHagIuDCYA4So/QlMXkT";
                //string keyPem = @"MIIEowIBAAKCAQEAwWg/QgMYRf39SgOKeP30m9WFndYzzvwevmOQt04RceAv9nUsM87kdcwVhxU1pJPJlEHWv/PE1ePU/d005IxYfyBDhxEC3bVRrS4imlx7TcBEtC7DyeNzwOSOXwJAGK4kFUCfC7RYNIC/zaNIBtLxgTgcIt/3I53T0eJEhkTDxfyutHr4\r\nizv2I2n61S04mU2Rp0+BR7w18QlXStTzoXimHuMnksmgtCf8XN7l6LHZnHYDlFeYi/TEsjdSqJstCUoIs/voKtZZNnXlKXETz8jr8ncU2yKNDsaaEge7s1x3gmOj01O0uG+qwT8Ux241kg6WFZi0dCgNOFSfXDgdWU+LqwIDAQABAoH/VZ2/mqtf4VlF7fKjVFuGuu/FGqULjP74htUSFBMnmqs53h1v6Lb18RlLIHCWogcM7MaRh2eqUTGEjNzfnfPQydoUvKUepT5WlkKNtdKTzhSia5BIG61if88zIOb580DgrSOaqJlUGZDO0fD2GH6LrQbWNhR0mzPZBbzQ0HLM6YPvibIi43cCFvIL/bi5Sy7EhbPT3smijud65PpXEduzTEsr98wrHYijGTeJwIcUDVucsuZOJzWmFhZ5HQaUStY9mo9RJ3ZRt+IPOPaGAvd+SM9Qka/a9T+y9zL4RX1tpewOLWq0ZUqSd97WVyAiwyK3+0Gm1q7PMazdKJdi\r\n3+4BAoGBAOmz2Xp5sryvYSSLcy8kSbqRBGSTbkfyu9NCz8Hp6GhwkWoNCFVt9WIm\r\nmNhdf8mtf2JgRvAmc4aA+sC/+Q89TXxXTI2Sx6BMPINEfJ5+Nblaf7tIflq0qcGi\r\nlherMBykyEPb708HJBCTX4nxad+d2ajuwnsA2mpXibeomZMoc1WrAoGBANPcMLDa\r\nHDFGw28PEx/dbvQWwPd2qLfuDsR5TylC0+QMt+d1hUVIikd2AHP0bsySyNVsqPtF\r\nD13vcAxfIjJkKkqkcMXagiMe5vDnaKHeMXYwJ5h1hEA3gHl3lI4ZG7Kz2/u1Z5wv\r\nHn7gzhs6ekxJoX+GEHXVvuOJakbnlBOr8qIBAoGBANgqRfAWXzBOHFGcNyeUrinG\r\nd0S8ZdZg/EC13MulmxX3gVmRwnKaqc1mnBwNp+SY+Wm6WgNJ3bO827OsHihtcJ2h\r\ndirBaKqpuZNFputHHFRdFCbcMBugXiDATYqJCpCEDsVNyrXTihVW65kBVwfCzxCG\r\nABecO2r/m1/osFzAnqXPAoGBALgxDZwUZMJqtWsO/h+MEvTf+3Cql1EiBXDHzw0e\r\nbr4sWI9BW1a7FI3Ah/BIKTRuwXDcgaXDivKuQwpeg4qwDxzKDsnViaMjMMFtT0i7tuD8SNCCCzMRjf8M+fj4113shWs3chcV2DuTE14U31MICQKn+hOYe9ezFtUCbXkU22QBAoGBAMqQC3bBC+Toool94cV81aA5+f62svVydd7dLkm6dsEXL+mee42rcCJ4EfWVu5lUaj9iXXrKPPHxM71eHtn/b1E1/k7NAmlKdWfcG9CDVFrYfBi3z/21VKdkwRpRChG6V8uTGtO964g7S7RzD8fxIfBHowUDLR39mh0BI1Kp7Z7W";
                string combinedPem = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAtdz61BYowZlKCi2QKx/Zl1V5nTsdKIXmIy0xyCysaXPahNW6
sfDH64VXzKtICUKiM929i0mCF+EzKhQZkV2Rq/VJfK+tdtQTUEb56r9uzlrppKH8
Kg5sYTUeZVWDcwYx65ClAZfOmrAmb9iTDa6sedcZluIH3jN3IZAkpnxk3DTo6Hgs
307K/aPYLw/liG8TNGRGSAWR2/O3w3xk7zsU6xZD3KFu4EQ2J3rq+s7vRsqyeLND
mnv5Ao49pu1zjWrsC45JTxMFCP09v7P+IBevHiobIelTiQb4j/Drg8XUGM57EIEL
iowzfsnQfjCp7+DAwzwplU2An8t8wY21VDUtDwIDAQABAoIBAAk1ysIo6SF82OIR
EqcMcuGWI5WR49UkitYnOMkxcf3ZLyzhAhZ24jkMRLkJx5kodbx4s1u1elApSIUO
BYl2GqWBw5n776X/8YZ0Qb3gugTbV8/NoE6k8lot7X8mSITvwGF0rtaY0I7G8vth
FXlw6i01dodMpSdcpeDeQuznidgZA8Lui7gDOC47AuoyaqXJJUCbh5NdC7GtBi5Z
hZWQi1W5L8OpZYiIrKSjw/xkNUFDvcxYlSOPaLD9T8+dubJ9WHmnQ1n/0zo8ftWs
TzizFdmJp8NK/XXd0I7o64r7FE3Dzdes+dY1ZN5hhXy4Nps1tNjAxgjXs8RUVeL9
pYPXz3kCgYEA+zStQtm3G5kgzFYAqr3y+6OPzUtEGVDlalqOfYMba/PCQpIHFwwd
jcyraQ1omIH6ycLYzllYShDZzC/V05xKZ6JPN6ijzXeht/OmeQqFwp69fXpYG95/
ZVU2ovFAIdmVWMjZRtIATsaEb6VGSJuIFLqnZIhvjXf7BOyZXgEmbGUCgYEAuVWD
mCRfcPVZsUd8iYbbMUUGv5IwYbvTJ0crJtFjPDCYxNT29pIv+hcOtQ2z9e6eJ30h
2DQP11SWYJQ06R+OljqGVpXqJb7i9ds8lAwPJK29z/LKKN4vxDqw1qgSNU5BatLk
WrpNfOr+WafOR3UuKwiwA3q5CKw90vnvwFcLGmMCgYEArM0NqoDNfsSVVTqL9uf3
b7aRwyzVSXzmc4SbpCc+A8KIjPaavfXtlo+GEhvRKaev9n58WWYIX9q7l8QIGSrX
2/ZA/A6br0luC3Ylu/nLDZfYzaoNVg52ICTAEtJVxXNBUvPG0vq+j3HrxEky0dt7
fpbvcy6PXgd8u2RuE8ZCwikCgYAi3ZSBBa6yyGVUNXeXQ7hun2aymWTirfv+MqRK
jC+X/ihvNwUyw7Ok37uTkf1d8cLs+NxqGPk18CLODnOZu6cOWfo1xcQjz7JQwCq4
S0B6DPldanZfk9T2nHfyGfI69vrVhtEQxGWZb3fkE+55gPDrhZnfPbIbkJOc4goZ
bAaZswKBgQCzKtaX4UwofV3FdwunU3U1AkU2qeOj58uptA3Fcv93xQWy/MYtb3ei
OrCFyND7ec0FwSvzBIQY8R43/AhtgRcY45Wr4n3wZeFULm+NrV0wcnHLehFYLhKQ
DaQTGHaX5wH5XSxoMLDb2aahDzQhmyxwf3yqn6cosrgoB906mxC/fA==
-----END RSA PRIVATE KEY-----
-----BEGIN CERTIFICATE-----
MIIEnzCCA4egAwIBAgIQMzpa7LDU+47IgQxkiubCYzANBgkqhkiG9w0BAQsFADB/
MQswCQYDVQQGEwJVUzESMBAGA1UECgwJVmlzYSBJbmMuMS8wLQYDVQQLDCZWaXNh
IEludGVybmF0aW9uYWwgU2VydmljZSBBc3NvY2lhdGlvbjErMCkGA1UEAwwiVmlz
YSBOb24tUHJvZCBVUy1FYXN0LTEgSXNzdWluZyBDQTAeFw0yNTAzMDUyMzMxNTha
Fw0yNzA1MjUwMDMxNThaMIGPMTQwMgYDVQQDDCtNS0wwOTguY2VmNjQ2NGEtMjRi
MS00ZDllLTg1NTktOGNiNjQ3MzQxOWE2MQwwCgYDVQQLDANNQVAxDTALBgNVBAoM
BFZpc2ExDzANBgNVBAcMBk11bmljaDELMAkGA1UECAwCQlkxCzAJBgNVBAYTAkRF
MQ8wDQYDVQQFEwZNS0wwOTgwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB
AQC13PrUFijBmUoKLZArH9mXVXmdOx0oheYjLTHILKxpc9qE1bqx8MfrhVfMq0gJ
QqIz3b2LSYIX4TMqFBmRXZGr9Ul8r6121BNQRvnqv27OWumkofwqDmxhNR5lVYNz
BjHrkKUBl86asCZv2JMNrqx51xmW4gfeM3chkCSmfGTcNOjoeCzfTsr9o9gvD+WI
bxM0ZEZIBZHb87fDfGTvOxTrFkPcoW7gRDYneur6zu9GyrJ4s0Oae/kCjj2m7XON
auwLjklPEwUI/T2/s/4gF68eKhsh6VOJBviP8OuDxdQYznsQgQuKjDN+ydB+MKnv
4MDDPCmVTYCfy3zBjbVUNS0PAgMBAAGjggEEMIIBADAgBgNVHREEGTAXghVNS0ww
OTguY3licy5zZWNsaWIuaW8wCQYDVR0TBAIwADAfBgNVHSMEGDAWgBT/fbF1pZjr
qGxJZ51PFi2Y8aKk/DAdBgNVHQ4EFgQUVAwA1foV1VmcO2S+ErAOiiLN8eQwDgYD
VR0PAQH/BAQDAgWgMB0GA1UdJQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjBiBgNV
HR8EWzBZMFegVaBThlFodHRwOi8vZDEwNmFsdmU3a2hoM2UuY2xvdWRmcm9udC5u
ZXQvY3JsLzI5NTRjNGE5LTI3YzItNDBmMy1hYzE4LThhNTJjZDI5NTlkNi5jcmww
DQYJKoZIhvcNAQELBQADggEBAKoPZMtkWW/ZSZtolX1mniv+dpynZcwK+BxNGhGx
NCzBDDXGeFHxdSEzpx4qSZ/+Y01+so0XL8EvdujIYZMhij2hRJ/c7dIGRyt0TUUF
7HSlTdTfE/hKew3Mxvl7aqq5npS3vZVVXW1fFLQk60uyuOclKtKEhQTCKZ7zcNtG
UKJppfFThau1voYhZ6hEzFJcTH4nIYAYET4mBvT5AeNfFDSykK/38Y/cJFx7y3rl
HkVgRZr9MbuwusjprBLkRUTKcWeUUNFkfaEYc0KzUiyuFSMmQ3GSCkCvbp+YUdm0
HVmuv/pRK7cyt58Ar9ecRjqjatnw87E3QJwZY7Lca5PcoEc=
-----END CERTIFICATE-----
-----BEGIN CERTIFICATE-----
MIIEqjCCA5KgAwIBAgIUBKdnhadG0JlZaZbmLUvdq/44gt8wDQYJKoZIhvcNAQEL
BQAwfTELMAkGA1UEBhMCVVMxDTALBgNVBAoMBFZJU0ExMDAuBgNVBAsMJ1Zpc2Eg
SW50ZXJuYXRpb25hbCBTZXJ2aWNlcyBBc3NvY2lhdGlvbjEtMCsGA1UEAwwkVmlz
YSBOb24tUHJvZCBDb3Jwb3JhdGUgUm9vdCBDQSAtIEcyMB4XDTIxMDYwOTIwNDM0
MloXDTQwMDcxMDIxNTYxOFowfzELMAkGA1UEBhMCVVMxEjAQBgNVBAoMCVZpc2Eg
SW5jLjEvMC0GA1UECwwmVmlzYSBJbnRlcm5hdGlvbmFsIFNlcnZpY2UgQXNzb2Np
YXRpb24xKzApBgNVBAMMIlZpc2EgTm9uLVByb2QgVVMtRWFzdC0xIElzc3Vpbmcg
Q0EwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC/RAQGJRwe/AXpkyLH
T1QZWIqjNo4jI9vjeLKuQ+DTUNnT8d3iewZm273jToF5kIi1LLx2rPFe6j36D9cD
GZYKAR4OtiEqLabFMlO23BtGeKOUfPsYCpH2W1ao2IX7O/InVRbwon0D05pP7fJz
9bH3QsaEqh/PYB4gV9xeofOSKEklouD8K+iG410Q+6gSXv6eYZ/6K93TorHecKGp
qRa6bJDrHIVr3wbaohD1xe/ZOLKc4y9RbS24l4JPCRIBNdXpKdCKtLh+91FpvJFG
srhFYqjW+s82o9yQvN7+K5JjoV6l5cvbN9A/Iizi3PCLAIIbrJFeiV2w6Y2Z3oXA
JCsDAgMBAAGjggEeMIIBGjASBgNVHRMBAf8ECDAGAQH/AgEBMB8GA1UdIwQYMBaA
FCYyUt971g6d9Bmekwg7TIvkFziEMGEGCCsGAQUFBwEBBFUwUzAtBggrBgEFBQcw
AoYhaHR0cDovL2FpYXFhLnZpc2EuY29tL3ZucGNyZzIuY2VyMCIGCCsGAQUFBzAB
hhZodHRwOi8vb2NzcHFhLnZpc2EuY29tMB0GA1UdJQQWMBQGCCsGAQUFBwMCBggr
BgEFBQcDATAyBgNVHR8EKzApMCegJaAjhiFodHRwOi8vYWlhcWEudmlzYS5jb20v
dm5wY3JnMi5jcmwwHQYDVR0OBBYEFP99sXWlmOuobElnnU8WLZjxoqT8MA4GA1Ud
DwEB/wQEAwIBBjANBgkqhkiG9w0BAQsFAAOCAQEAKwWtJDIwyNhvgVpzfuo0mXeC
607NaXedvCzyc8ffvsYhNbgvLrggigdDoO7ZcIB1DojIbIIDsVTQ56IkhUPxbtQM
x5KyNiRuFa8+5lUlx8HEQlgwoVQ/xXyMEzYy4x633vgbLrGS/8wfOH5jsKo9sKub
7rdhuyG/FsBQV+uHtLMEAP9P1he87hb9ZkMw4aNM6N+GYNJrftfI/6+sWvtoFDo8
MWHGIMNwmLZOemlYczROX8KsdfX5BliMTj3LliBG6ks2jb4ZwMltsepTcKPWlkoB
eFWwDFI16S6hGGurgbUZS2MvuYb2SrEDhKq1Hw7v94bNXDBvgtAgMZ/LkcREGg==
-----END CERTIFICATE-----
-----BEGIN CERTIFICATE-----
MIIDyjCCArKgAwIBAgIUPa0jiSEfwWVOLYc2gBYMzkdyzyQwDQYJKoZIhvcNAQEL
BQAwfTELMAkGA1UEBhMCVVMxDTALBgNVBAoMBFZJU0ExMDAuBgNVBAsMJ1Zpc2Eg
SW50ZXJuYXRpb25hbCBTZXJ2aWNlcyBBc3NvY2lhdGlvbjEtMCsGA1UEAwwkVmlz
YSBOb24tUHJvZCBDb3Jwb3JhdGUgUm9vdCBDQSAtIEcyMB4XDTIwMDcxNTIxNTYx
OFoXDTQwMDcxMDIxNTYxOFowfTELMAkGA1UEBhMCVVMxDTALBgNVBAoMBFZJU0Ex
MDAuBgNVBAsMJ1Zpc2EgSW50ZXJuYXRpb25hbCBTZXJ2aWNlcyBBc3NvY2lhdGlv
bjEtMCsGA1UEAwwkVmlzYSBOb24tUHJvZCBDb3Jwb3JhdGUgUm9vdCBDQSAtIEcy
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxkfhaITgThpByNEhCjv/
eVH7iQdF70AoFXWX2rEdTDcrIX9qwLpH4DFMYVNM4Mgq9UI3hL5tf3C0A5za3vjx
ns4KLVBEJB+cXrX44vzHEsugkd74iJZsJvyvIJQ/TNuYog/wb+JC1lLLHnmTLBny
FgOcRMbcz33EBpvTO8j0ExGrp9ZOKhpca0haV0dGqUxrUyzZuDj1ij+q16YrP9UY
aVV56ipEVKcP2RgZCK69ngmVM2JRUmlVbBxVqBqM1FigfPWShic8joF6tcqTdcY2
3dc8/t643UrcbeIHTJ8In7FwqgZV1eOSAG+0nBRqgUbidNyabqy8cckZ4VusBs17
dwIDAQABo0IwQDAPBgNVHRMBAf8EBTADAQH/MB0GA1UdDgQWBBQmMlLfe9YOnfQZ
npMIO0yL5Bc4hDAOBgNVHQ8BAf8EBAMCAQYwDQYJKoZIhvcNAQELBQADggEBAFHj
xqGgy3h8S+f8PdaBagNkefPbV3fleKQDcy2bmdDxc08RaRMByDdo9YEXhJS6x1JL
U/g/yLFzp4ONpVeyHO8SIljw88/NAOTGvUbXAEU635hHQQWCYYBlOjSxeNUDrg/Y
SE8FCa8SEX/MRvfGiqEnCWfVZdRVwYMsjeL86eR4lq+GWT96/kCfBbG21da06eWU
Ujx3x+XLzeVG1nq40YIx0HwJQy1cL7JO550zwqh/OJ2rwleseb8LGIs38PtuL4uv
loPZYw1a/umzxveiWTZ8bU/df8dXXHxHSo24CCwO2lQy/aYZgJ6Z8NFGUzn4+tD+
ThWMgsrjWdv8fCbQSE0=
-----END CERTIFICATE-----";

                if (string.IsNullOrEmpty(combinedPem) || string.IsNullOrEmpty(combinedPem))
                {
                    Console.Error.WriteLine("Certificate PEM or Key PEM not provided.");
                    return ("Certificate PEM or Key PEM not provided.");
                }
                X509Certificate2 clientCert = null!;
                try
                {
                    clientCert = X509Certificate2.CreateFromPem(combinedPem, combinedPem);
                    // Alternatively, load from PFX:
                    // clientCert = new X509Certificate2("myClient.pfx", "pfxPassword", X509KeyStorageFlags.EphemeralKeySet);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load certificate: {ex.Message}");
                    return $"Failed to load certificate: {ex.Message}";
                }
                

                try
                {
                    Console.WriteLine($"Outbound reqest string: {request}\n");
                    ws.Options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    Console.WriteLine("Connecting to Server - wss://192.168.50.178:8443");

                    await ws.ConnectAsync(new Uri("wss://192.168.50.178:8443"), CancellationToken.None);

                    Console.WriteLine("Connected");

                    Console.WriteLine($"Outbound reqest string: {request}");

                    await SendJsonOverWebSocket(request, ws);

                    await ReceiveMessages(ws);

                    // Properly close the WebSocket
                    if (ws.State == WebSocketState.Open)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done sending data", CancellationToken.None);
                        Console.WriteLine("WebSocket closed");
                    }
                    return ws.State.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return $"An error occurred: {ex.Message}";
                }

            }
        }

        private static async Task SendJsonOverWebSocket(string json, WebSocket webSocket)
        {

            if (webSocket.State == WebSocketState.Open)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private static async Task<string> ReceiveMessages(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 8];
            WebSocketReceiveResult result;
            string receivedMessage = string.Empty;

            try
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Message received from server: {receivedMessage}");

                    // Properly close the WebSocket
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done sending data", CancellationToken.None);
                        Console.WriteLine("WebSocket closed");
                    }

                    return receivedMessage;

                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Receive operation timed out.");
                receivedMessage = "Receive operation timed out.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (receivedMessage is not null)
                {
                    return receivedMessage;
                }
                else
                {
                    receivedMessage = $"An error occurred: {ex.Message}";
                    return receivedMessage;
                }
                    
            }

            return receivedMessage;
        }
    }
    
}
