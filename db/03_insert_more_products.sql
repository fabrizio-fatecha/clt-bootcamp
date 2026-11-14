WITH categories (category_index, name, description, min_price, max_price) AS (
    VALUES
        (0, 'Notebook',    'Computadora portatil para trabajo y estudio',        600, 2500),
        (1, 'Monitor',     'Monitor para escritorio con panel IPS',              120,  900),
        (2, 'Teclado',     'Teclado ergonomico para oficina y gaming',            20,  250),
        (3, 'Mouse',       'Mouse inalambrico de alta precision',                 10,  150),
        (4, 'Auriculares', 'Auriculares con microfono y cancelacion de ruido',    15,  400),
        (5, 'Webcam',      'Camara web Full HD con microfono integrado',          25,  300),
        (6, 'SSD',         'Unidad de estado solido NVMe de alta velocidad',      40,  350),
        (7, 'Memoria RAM', 'Modulo de memoria DDR5 para equipos de escritorio',   25,  250),
        (8, 'Router',      'Router Wi-Fi 6 de doble banda',                       30,  300),
        (9, 'Impresora',   'Impresora multifuncion con conexion inalambrica',     90,  700)
),
brands (brand_index, name) AS (
    VALUES
        (0, 'Lenovo'), (1, 'Dell'), (2, 'HP'), (3, 'Asus'), (4, 'Acer'), (5, 'Logitech'),
        (6, 'Samsung'), (7, 'Kingston'), (8, 'TP-Link'), (9, 'Epson'), (10, 'Apple'), (11, 'MSI')
)
INSERT INTO public.products (name, description, price, stock)
SELECT
    category.name || ' ' || brand.name || ' ' || lpad(series.product_number::text, 6, '0'),
    category.description || ', marca ' || brand.name,
    round((category.min_price + random() * (category.max_price - category.min_price))::numeric, 2),
    floor(random() * 201)::int
FROM generate_series(21, 100000) AS series(product_number)
JOIN categories AS category ON category.category_index = series.product_number % 10
JOIN brands AS brand ON brand.brand_index = (series.product_number / 10) % 12
ON CONFLICT ((lower(name))) DO NOTHING;
