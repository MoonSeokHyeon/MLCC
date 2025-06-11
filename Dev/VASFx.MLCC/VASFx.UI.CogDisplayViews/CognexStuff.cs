using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.UI.CogDisplayViews
{
    public class CognexStuff
    {
        public ICogImage Convert8BitRawImageToCognexImage( byte[] imageData, int width, int height)
        {
            // no padding etc. so size calculation
            // is simple.
            var rawSize = width * height;

            var buf = new SafeMalloc(rawSize);

            // Copy from the byte array into the
            // previously allocated. memory
            Marshal.Copy(imageData, 0, buf, rawSize);

            // Create Cognex Root thing.
            var cogRoot = new CogImage8Root();

            // Initialise the image root, the stride is the
            // same as the widthas the input image is byte alligned and
            // has no padding etc.
            cogRoot.Initialize(width, height, buf, width, buf);

            // Create cognex 8 bit image.
            var cogImage = new CogImage8Grey();

            // And set the image roor
            cogImage.SetRoot(cogRoot);

            return cogImage;
        }
    }
}
