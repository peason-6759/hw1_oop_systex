# hw1_oop_systex
A simple C# object-oriented programming (OOP) task is to convert a text file to SQL and display it in a DataGridView. To handle multiple folders efficiently, multithreading is used for each task, respectively.

## Contents
1. TBD
   1. [將Insert into及create table由Base class完成](#example)
   2. [防呆機制](#example)
   3. [處理例外檔案，對於完全不同檔名](#example)
   4. [可一次丟入多種主標題一致的檔案](#example)
3. 已知問題
    a. [multithreading 下的mysql連接問題](#example)
   
        1. 兩執行續同時進db導致 => System.NotSupportedException: 'The ReadAsync method cannot be called when another read operation is pending.'
        2. 兩個同時連線導致Connection失效

