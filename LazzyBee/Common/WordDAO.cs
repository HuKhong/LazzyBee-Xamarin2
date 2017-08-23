using System;
using SQLite;
using System.ComponentModel;
namespace LazzyBee
{
	[Table("vocabulary")]
	public class WordDAO : INotifyPropertyChanged
	{
		private int _id;
		[PrimaryKey, AutoIncrement]
		public int id
		{
			get
			{
				return _id;
			}
			set
			{
				this._id = value;
				OnPropertyChanged(nameof(id));
			}
		}

		private string _question;
		[NotNull]
		public string question
		{
			get
			{
				return _question;
			}
			set
			{
				this._question = value;
				OnPropertyChanged(nameof(question));
			}
		}

		private string _answers;
		[NotNull]
		public string answers
		{
			get
			{
				return _answers;
			}
			set
			{
				this._answers = value;
				OnPropertyChanged(nameof(answers));
			}
		}

		private string _category;
		[NotNull]
		public string category
		{
			get
			{
				return _category;
			}
			set
			{
				this._category = value;
				OnPropertyChanged(nameof(category));
			}
		}

		private string _subcats;
		[NotNull]
		public string subcats
		{
			get
			{
				return _subcats;
			}
			set
			{
				this._subcats = value;
				OnPropertyChanged(nameof(subcats));
			}
		}

		private string _tags;
		[NotNull]
		public string tags
		{
			get
			{
				return _tags;
			}
			set
			{
				this._tags = value;
				OnPropertyChanged(nameof(tags));
			}
		}
		private string _related;
		[NotNull]
		public string related
		{
			get
			{
				return _related;
			}
			set
			{
				this._related = value;
				OnPropertyChanged(nameof(related));
			}
		}

		private Int64 _gid;
		[PrimaryKey, AutoIncrement]
		public Int64 gid
		{
			get
			{
				return _gid;
			}
			set
			{
				this._gid = value;
				OnPropertyChanged(nameof(gid));
			}
		}

		private int _status;
		[PrimaryKey, AutoIncrement]
		public int status
		{
			get
			{
				return _status;
			}
			set
			{
				this._status = value;
				OnPropertyChanged(nameof(status));
			}
		}

		private int _queue;
		[PrimaryKey, AutoIncrement]
		public int queue
		{
			get
			{
				return _queue;
			}
			set
			{
				this._queue = value;
				OnPropertyChanged(nameof(queue));
			}
		}

		private string _package;
		[NotNull]
		public string package
		{
			get
			{
				return _package;
			}
			set
			{
				this._package = value;
				OnPropertyChanged(nameof(package));
			}
		}

		private int _level;
		[PrimaryKey, AutoIncrement]
		public int level
		{
			get
			{
				return _level;
			}
			set
			{
				this._level = value;
				OnPropertyChanged(nameof(level));
			}
		}

		private int _due;
		[PrimaryKey, AutoIncrement]
		public int due
		{
			get
			{
				return _due;
			}
			set
			{
				this._due = value;
				OnPropertyChanged(nameof(due));
			}
		}

		private int _rev_count;
		[PrimaryKey, AutoIncrement]
		public int rev_count
		{
			get
			{
				return _rev_count;
			}
			set
			{
				this._rev_count = value;
				OnPropertyChanged(nameof(rev_count));
			}
		}

		private string _user_note;
		public string user_note
		{
			get
			{
				return _user_note;
			}
			set
			{
				this._user_note = value;
				OnPropertyChanged(nameof(user_note));
			}
		}

		private int _last_ivl;
		[PrimaryKey, AutoIncrement]
		public int last_ivl
		{
			get
			{
				return _last_ivl;
			}
			set
			{
				this._last_ivl = value;
				OnPropertyChanged(nameof(last_ivl));
			}
		}

		private int _e_factor;
		[PrimaryKey, AutoIncrement]
		public int e_factor
		{
			get
			{
				return _e_factor;
			}
			set
			{
				this._e_factor = value;
				OnPropertyChanged(nameof(e_factor));
			}
		}

		private string _l_vn;
		public string l_vn
		{
			get
			{
				return _l_vn;
			}
			set
			{
				this._l_vn = value;
				OnPropertyChanged(nameof(l_vn));
			}
		}

		private string _l_en;
		public string l_en
		{
			get
			{
				return _l_en;
			}
			set
			{
				this._l_en = value;
				OnPropertyChanged(nameof(l_en));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
