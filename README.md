## ComputerGraphics Filters

Лабораторная работа по курсу компьютерной графики.

Выполнили: [Damlrca](https://github.com/Damlrca), [Mortus19](https://github.com/Mortus19), [aartyomm](https://github.com/aartyomm)

### Примеры фильтров

***Инверсия***

![borsch](sample_images/borsch.png)
![inverted borsch](sample_images/borsch_invert.png)

***Размытие по Гауссу***

![butterfly](sample_images/butterfly.png)
![gaussian blur butterfly](sample_images/butterfly_gauss.png)

***Выделенме границ (Оператор Собеля)***

![conrod](sample_images/conrod.png)
![sobel operator conrod](sample_images/conrod_sobel.png)

***Тиснение***

![columns](sample_images/columns.png)
![embossing columns](sample_images/columns_embossing.png)

***Медианный фильтр (подавление шума "соль и перец")***

![noisy crab](sample_images/crab_noisy.png)
![median filter crab](sample_images/crab_median.png)

***Линейное растяжение гистограммы***

![snowtree](sample_images/snowtree.png)
![autolevels snowtree](sample_images/snowtree_autolevels.png)

***Оттенки серого + Дизеринг***

![scott](sample_images/scott.png)
![dithering scott](sample_images/scott_dither.png)

### Список реализованных фильтров

- Точечные фильтры
	- Инверсия
	- Оттенки серого
	- Сепия
	- Увеличение яркости
- Матричные фильтры
	- Размытие
		- Размытие в движении
		- Размытие по Гауссу
	- Выделение границ
		- Оператор Прюитт
		- Оператор Собеля
		- Оператор Щарра
	- Повышение резкости
	- Тиснение
- Геометрические фильтры
	- Поворот
	- Перенос
	- Волны
	- Стекло
- Нелинейные фильтры
	- Медианный
	- Максимальный
	- Минимальный
- Глобальные фильтры
	- Повышение контрастности
	- Линейное растяжение гистограммы
	- Серый мир
	- Идеальный отражатель
	- Коррекция с опорным цветом
- Дизеринг
- Морфологические фильтры
