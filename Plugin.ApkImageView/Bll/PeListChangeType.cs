using System;

namespace Plugin.ApkImageView.Bll
{
	/// <summary>Тип изменения файла</summary>
	public enum PeListChangeType
	{
		/// <summary>Неизвестный тип изменения</summary>
		None = 0,
		/// <summary>Файл добвлен</summary>
		Added = 1,
		/// <summary>Файл удалён</summary>
		Removed = 2,
		/// <summary>Файл изменён извне</summary>
		Changed = 3,
	}
}