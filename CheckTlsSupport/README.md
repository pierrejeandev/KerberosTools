# Check TLS Support Tool

Displays the supported/enabled version of TLS on the installed DotNet Framework.

On older DotNet framework (before 4.0) TLS 1.1 and 1.2 can be supported but they are not enabled  by default. This can be an issue when you have to maintain an old application running on DotNet 2.0 and need support for TLS 1.2..

This tool displays the Active TLS version and the effective version of DotNet.

You can enable TLS 1.1 and 1.2 on older DotNet Framework with:

```C#
// We use ids here instead of SecurityProtocolType enum to support build on old DotNet version where SecurityProtocolType.Tls12 does not exists 
ServicePointManager.SecurityProtocol =  (SecurityProtocolType)(
  (int)SecurityProtocolType.Ssl3 
  | 3072 // SecurityProtocolType.Tls12 
  | 768 // SecurityProtocolType.Tls11 
  | 192 // SecurityProtocolType.Tls;
  );
```

Note: SecurityProtocolType.Tls12 does not exists in DotNet 2.0 but still we can support TLS 1.2 on DotNet 2.0 application with this trick, if we are using a version of windows that support it (TLS is managed by Windows disrectly, not, by DotNet frameworks).
As of 2020 all supported version of windows, include support for TLS 1.2. 



