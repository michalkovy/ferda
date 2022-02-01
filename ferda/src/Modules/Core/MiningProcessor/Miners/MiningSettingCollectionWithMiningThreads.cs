// MiningSettingCollectionWithMiningThreads.cs - Collection of mining threads to do
// distributed mining
//
// Authors:  Michal Kováè <michal.kovac.develop@centrum.cz>    
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System.Collections.Generic;
using System.Threading;

namespace Ferda.Guha.MiningProcessor
{
	public class MiningSettingCollectionWithMiningThreads<Type>
	{
		private Queue<Type> miningSetting = new Queue<Type>();
		private Mine mine;
		private Finished finished;
		private Semaphore semaphore;
		private Semaphore semaphore2;
		private bool runThreads = true;
		private List<Thread> threads = new List<Thread>();
		
		public delegate bool Finished();
		public delegate void Mine(Type t);
		
		public MiningSettingCollectionWithMiningThreads(Mine m, Finished f)
		{
			mine = m;
			finished = f;
			semaphore = new Semaphore(0, 2 * System.Environment.ProcessorCount);
			semaphore2 = new Semaphore(2 * System.Environment.ProcessorCount, 2 * System.Environment.ProcessorCount);
			for (int i = 0; i < System.Environment.ProcessorCount; i++)
			{
				Thread thread = new Thread(threadDo);
				thread.Priority = ThreadPriority.AboveNormal;
				thread.Start();
				threads.Add(thread);
			}
		}
		
		private void threadDo()
		{
			bool runThreadsLocal;
            int clearCount = 0;
			
			do
			{
				semaphore.WaitOne();
                semaphore2.Release();
				Type t;
				lock (this)
				{
					runThreadsLocal = (runThreads || miningSetting.Count > 0) && !finished();
					if (runThreadsLocal)
					{
						t = miningSetting.Dequeue();
					}
					else
					{
						clearCount = miningSetting.Count;
						miningSetting.Clear();
						continue;
					}
				}
				mine(t);
			}
			while(runThreadsLocal);
            for (int i = 0; i < clearCount; i++)
            {
                semaphore.WaitOne();
                semaphore2.Release();
            }
		}
		
		public void AddSetting(Type t)
		{
		    semaphore2.WaitOne();
            lock (this)
            {
				if (runThreads)
				{
					miningSetting.Enqueue(t);
					semaphore.Release();
				}
				else
				{
					semaphore2.Release();
				}
            }
		}
		
		public void Finish()
		{
			lock (this)
			{
				runThreads = false;
			}
			for (int i = 0; i < System.Environment.ProcessorCount; i++)
			{
				semaphore2.WaitOne();
				semaphore.Release();
			}
			
			foreach (Thread thread in threads)
			{
				thread.Join();
			}
		}
	}
}
