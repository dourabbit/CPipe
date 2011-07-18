from berniea.ironPython.extending.ClassLib import *
print 'Rule1'

if saleBasket.Total > 100:
	discount = saleBasket.Total * -0.1
	saleBasket.Lines.Add(Line(ProductName="Dummy", ProductPrice=discount, Quantity=1, Amount=discount))
	print 'discount given: ' + (discount * -1).ToString()