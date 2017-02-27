using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace MSCorp.FirstResponse.Client.UWP.Extensions
{
    public static class RandomAccessStreamReferenceExtensions
    {
        // http://stackoverflow.com/questions/39618846/xamarin-forms-image-to-from-irandomaccessstreamreference/39632398#39632398
        public static async Task<RandomAccessStreamReference> ScaleTo(this IRandomAccessStreamReference imageStream, uint width, uint height)
        {
            using (IRandomAccessStream fileStream = await imageStream.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(fileStream);

                //create a RandomAccessStream as output stream
                var memStream = new InMemoryRandomAccessStream();

                //creates a new BitmapEncoder and initializes it using data from an existing BitmapDecoder
                BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(memStream, decoder);

                //resize the image
                encoder.BitmapTransform.ScaledWidth = width;
                encoder.BitmapTransform.ScaledHeight = height;
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Cubic;

                //commits and flushes all of the image data
                await encoder.FlushAsync();

                //return the output stream as RandomAccessStreamReference
                return RandomAccessStreamReference.CreateFromStream(memStream);
            }
        }
    }
}
