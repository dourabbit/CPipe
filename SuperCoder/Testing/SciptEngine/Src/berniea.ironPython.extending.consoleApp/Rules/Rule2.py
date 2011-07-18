from berniea.ironPython.extending.ClassLib import *
print 'Rule2'

for line in saleBasket.Lines:
	
	if line.ProductName == 'Prod1':
		
		discount = line.Amount * 0.2
		line.Amount = line.Amount - discount
		print 'discount given: ' + discount.ToString()
	
	if line.Quantity >= 10:
		
		line.Amount = line.Amount - line.ProductPrice
		print 'discount given: ' + line.ProductPrice.ToString()