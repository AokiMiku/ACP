namespace ACP_GUI
{
	using System;
	using System.Windows.Media.Imaging;
	using System.IO;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class ByteToImageConverter
	{
		public static BitmapImage ConvertByteArrayToBitMapImage(byte[] imageByteArray)
		{
			BitmapImage img = new BitmapImage();
			img.BeginInit();
			img.StreamSource = new MemoryStream(imageByteArray);
			img.EndInit();
			return img;
		}
	}
}