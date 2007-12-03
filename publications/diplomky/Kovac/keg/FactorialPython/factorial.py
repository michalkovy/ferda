fac = lambda n:n-1 + abs(n-1)and fac(n-1)*n or 1L

fo = lambda x: x and fo(x-1)*long(x) or 1

factorial = lambda x: x == 0 and 1 or x * factorial(x - 1)

print f(0)
