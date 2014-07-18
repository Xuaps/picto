﻿using System;
using Windows.ApplicationModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace PictoTest
{
    [TestClass]
    public class StorageFileConverterSpec
    {
        [TestMethod]
        public void convertToBase64_should_return_a_base_64_image()
        {
            var file = Package.Current.InstalledLocation.GetFileAsync("assets\\logo.jpg").AsTask().Result;
            var base64 = StorageFileConverter.ConvertToBase64(file).Result;

            Assert.AreEqual("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cm9wV2hlblByaW50aW5nYm9vbAAAAAAOY3JvcFJlY3RCb3R0b21sb25nAAAAAAAAAAxjcm9wUmVjdExlZnRsb25nAAAAAAAAAA1jcm9wUmVjdFJpZ2h0bG9uZwAAAAAAAAALY3JvcFJlY3RUb3Bsb25nAAAAAAA4QklNA+0AAAAAABAASAAAAAEAAgBIAAAAAQACOEJJTQQmAAAAAAAOAAAAAAAAAAAAAD+AAAA4QklNBA0AAAAAAAQAAAB4OEJJTQQZAAAAAAAEAAAAHjhCSU0D8wAAAAAACQAAAAAAAAAAAQA4QklNJxAAAAAAAAoAAQAAAAAAAAACOEJJTQP1AAAAAABIAC9mZgABAGxmZgAGAAAAAAABAC9mZgABAKGZmgAGAAAAAAABADIAAAABAFoAAAAGAAAAAAABADUAAAABAC0AAAAGAAAAAAABOEJJTQP4AAAAAABwAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAADhCSU0EAAAAAAAAAgABOEJJTQQCAAAAAAAEAAAAADhCSU0EMAAAAAAAAgEBOEJJTQQtAAAAAAAGAAEAAAADOEJJTQQIAAAAAAAQAAAAAQAAAkAAAAJAAAAAADhCSU0EHgAAAAAABAAAAAA4QklNBBoAAAAAAz0AAAAGAAAAAAAAAAAAAACWAAAAlgAAAAQAbABvAGcAbwAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAlgAAAJYAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAQAAAAAAAG51bGwAAAACAAAABmJvdW5kc09iamMAAAABAAAAAAAAUmN0MQAAAAQAAAAAVG9wIGxvbmcAAAAAAAAAAExlZnRsb25nAAAAAAAAAABCdG9tbG9uZwAAAJYAAAAAUmdodGxvbmcAAACWAAAABnNsaWNlc1ZsTHMAAAABT2JqYwAAAAEAAAAAAAVzbGljZQAAABIAAAAHc2xpY2VJRGxvbmcAAAAAAAAAB2dyb3VwSURsb25nAAAAAAAAAAZvcmlnaW5lbnVtAAAADEVTbGlj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L25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoTWFjaW50b3NoKSIgeG1wOkNyZWF0ZURhdGU9IjIwMTItMTAtMThUMjI6MTY6NDYrMDI6MDAiIHhtcDpNZXRhZGF0YURhdGU9IjIwMTItMTAtMThUMjI6MTY6NDYrMDI6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDEyLTEwLTE4VDIyOjE2OjQ2KzAyOjAwIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjEwNDgyNEVCMDgyMDY4MTE4MDgzQkQ5RTEzRUFGMUVEIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjBGNDgyNEVCMDgyMDY4MTE4MDgzQkQ5RTEzRUFGMUVEIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6MEY0ODI0RUIwODIwNjgxMTgwODNCRDlFMTNFQUYxRUQiIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgZGM6Zm9ybWF0PSJpbWFnZS9qcGVnIj4gPHhtcE1NOkhpc3Rvcnk+IDxyZGY6U2VxPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0iY3JlYXRlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDowRjQ4MjRFQjA4MjA2ODExODA4M0JEOUUxM0VBRjFFRCIgc3RFdnQ6d2hlbj0iMjAxMi0xMC0xOFQyMjoxNjo0NiswMjowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoTWFjaW50b3NoKSIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6MTA0ODI0RUIwODIwNjgxMTgwODNCRDlFMTNFQUYxRUQiIHN0RXZ0OndoZW49IjIwMTItMTAtMThUMjI6MTY6NDYrMDI6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKE1hY2ludG9zaCkiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDxwaG90b3Nob3A6RG9jdW1lbnRBbmNlc3RvcnM+IDxyZGY6QmFnPiA8cmRmOmxpPnhtcC5kaWQ6MDI4MDExNzQwNzIwNjgxMTgyMkFGQkY2QkZBNDg0RDM8L3JkZjpsaT4gPC9yZGY6QmFnPiA8L3Bob3Rvc2hvcDpEb2N1bWVudEFuY2VzdG9ycz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPD94cGFja2V0IGVuZD0idyI/Pv/iDFhJQ0NfUFJPRklMRQABAQAADEhMaW5vAhAAAG1udHJSR0IgWFlaIAfOAAIACQAGADEAAGFjc3BNU0ZUAAAAAElFQyBzUkdCAAAAAAAAAAAAAAABAAD21gABAAAAANMtSFAgIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEWNwcnQAAAFQAAAAM2Rlc2MAAAGEAAAAbHd0cHQAAAHwAAAAFGJrcHQAAAIEAAAAFHJYWVoAAAIYAAAAFGdYWVoAAAIsAAAAFGJYWVoAAAJAAAAAFGRtbmQAAAJUAAAAcGRtZGQAAALEAAAAiHZ1ZWQA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"
                , base64);
        }
    }
}
