// Copyright (c) Files Community
// Licensed under the MIT License.

using Files.Core.Storage;
using Files.Core.Storage.Storables;
using Files.Shared.Utils;
using Windows.Storage.FileProperties;

namespace Files.App.Data.Contracts
{
	internal sealed class ImagingService : IImageService
	{
		/// <inheritdoc/>
		public async Task<IImage?> GetIconAsync(IStorable storable, CancellationToken cancellationToken)
		{
			if (storable is not ILocatableStorable locatableStorable)
				return null;

			var iconData = await FileThumbnailHelper.LoadIconFromPathAsync(locatableStorable.Path, 24u, ThumbnailMode.ListView, ThumbnailOptions.ResizeThumbnail);
			if (iconData is null)
				return null;

			var bitmapImage = await iconData.ToBitmapAsync();
			return new BitmapImageModel(bitmapImage);
		}

		public async Task<IImage?> GetImageModelFromDataAsync(byte[] rawData)
		{
			return new BitmapImageModel(await BitmapHelper.ToBitmapAsync(rawData));
		}

		public async Task<IImage?> GetImageModelFromPathAsync(string filePath, uint thumbnailSize = 64)
		{
			if (await FileThumbnailHelper.LoadIconFromPathAsync(filePath, thumbnailSize, ThumbnailMode.ListView, ThumbnailOptions.ResizeThumbnail) is byte[] imageBuffer)
				return await GetImageModelFromDataAsync(imageBuffer);

			return null;
		}
	}
}
