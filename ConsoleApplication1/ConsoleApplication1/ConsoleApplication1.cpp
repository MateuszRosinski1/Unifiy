#include <iostream>
#include <Windows.h>
#include <cstdlib>
#include <chrono>

int n;
int* arr = new int[n];
using std::chrono::high_resolution_clock;
using std::chrono::duration_cast;
using std::chrono::duration;
using std::chrono::milliseconds;
void BubbleSort()
{
	double dn = double(n);
	
	int t = 0 ,compares = 0;
	auto t1 = high_resolution_clock::now();
	for (int step = 0; step < n; ++step) {
		compares = 0;
		
		for (int i = 0; i < n - step; ++i) {

			
			if (arr[i] > arr[i + 1]) {				
				int temp = arr[i];
				arr[i] = arr[i + 1];
				arr[i + 1] = temp;
				compares++;
			}
		}
		t = t + compares;
		if (compares == 0)
			break;
	}
	auto t2 = high_resolution_clock::now();
	auto ms_int = duration_cast<milliseconds>(t2 - t1);
	duration<double, std::milli> ms_double = t2 - t1;
	std::cout << "Bubble t = " << t << std::endl;
	double qValue = (t / (dn*dn));
	std::cout << "Bubble Quotient value = " << qValue << std::endl;	
	std::cout << "Bubble Elapsed Time = " << ms_double.count() << std::endl;
	delete[] arr;
}

void InsertionSort() {

	double dn = double(n);
	
	double t = 0;
	int i, temp, j;
	auto t1 = high_resolution_clock::now();
	for (i = 1; i < n + 1; i++)
	{
		temp = arr[i];
		j = i - 1;


		while (j >= 0 && arr[j] > temp)
		{
			arr[j + 1] = arr[j];
			j = j - 1;
			t++;
		}
		arr[j + 1] = temp;		
	}
	auto t2 = high_resolution_clock::now();
	auto ms_int = duration_cast<milliseconds>(t2 - t1);
	duration<double, std::milli> ms_double = t2 - t1;
	std::cout << "[";
	for (int i = 0; i <= n; i++)
	{
		if (i != n)
			std::cout << arr[i] << ",";
		else
			std::cout << arr[i];
	}
	std::cout << "]" << std::endl;	
	double qValue = (t / (dn*dn));
	std::cout << "Insertion t = " << t << std::endl;
	std::cout << "Insertion Quotient value = " << qValue << std::endl;
	
	std::cout << "Insertion Elapsed Time = " << ms_double.count() << std::endl;
	delete[] arr;
}
void main()
{

	std::cout << "Would you like to use input array by yourself?\n (y/n)\n ";
	char choice;
	std::cout << "Input: ";  std::cin >> choice;
	std::cout << "Array lenght: ";
	std::cin >> n;


	if (choice == 'y')
	{
		int i = 0;
		do {
			std::cout << "array[" << i << "] = "; std::cin >> arr[i];
			i++;
		} while (i <= n);

	}
	else if (choice == 'n')
	{
		int i = 0;
		do {

			arr[i] = rand();
			i++;
		} while (i <= n);
		
	}
	else
	{
		system("cls");
		std::cout << "\n\n\n\n\n\n\n\n\n\n\n\n\n\                                                 Invalid Input";
		Sleep(3000);
		system("cls");
		main();
	}
	std::cout << "Which type of sort whould you like to do?\n (i/b)\n ";
	std::cout << "Input: ";  std::cin >> choice;
	if (choice == 'i')
	{
		
		InsertionSort();
		
	}
	else if (choice == 'b')
	{
		
		BubbleSort();
	}
	else {
		system("cls");
		std::cout << "\n\n\n\n\n\n\n\n\n\n\n\n\n\                                                 Invalid Input";
		Sleep(3000);
		system("cls");
		main();
	}

}