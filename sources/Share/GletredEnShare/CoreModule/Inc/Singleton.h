// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include <mutex>
#include "NonCopyable.h"

namespace GletredEngine
{
	template<typename T>
	class Singleton : public NonCopyable
	{
	public:
		static T* GetInstance()
		{
			std::lock_guard<std::mutex> lock(Mutex);
			static T instance;
			return &instance;
		}


	protected:
		Singleton() = default;
		~Singleton() override = default; 

		static std::mutex Mutex;
	};

	template<typename T>
	std::mutex Singleton<T>::Mutex;

}
