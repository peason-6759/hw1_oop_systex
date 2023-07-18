## hw1_oop_systex
A simple C# object-oriented programming (OOP) task is to convert a text file to SQL and display it in a DataGridView. To handle multiple folders efficiently, multithreading is used for each task, respectively.

1. TBD
   a. 將Insert into及create table由Base class完成
   b. 防呆機制
   c. 處理例外檔案，對於完全不同檔名
   c. 可一次丟入多種主標題一致的檔案
2.已知問題
  a. multithreading 下的mysql連接問題:
		//兩執行續同時進db導致 => System.NotSupportedException: ' The ReadAsync method cannot be called when another read operation is pending.'
		//兩個同時連線導致Connection失效
  
