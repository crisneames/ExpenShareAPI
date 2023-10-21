using System;
using Microsoft.Data.SqlClient;


namespace ExpenShareAPI.Repositories;

	public class BaseRepository
	{
		private readonly string _connectionString;

		public BaseRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		protected SqlConnection Connection
		{
			get
			{
				return new SqlConnection(_connectionString);
			}
		}
	}


