﻿using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _db;

		public OrderHeaderRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(OrderHeader obj)
		{
			_db.OrderHeaders.Update(obj);
		}

		void IOrderHeaderRepository.UpdateStatus(int id, string orderStatus)
		{
			var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);

			if (orderFromDb != null)
			{
				orderFromDb.OrderStatus = orderStatus;
			}
		}
	}
}