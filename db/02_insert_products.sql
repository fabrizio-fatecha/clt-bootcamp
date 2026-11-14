INSERT INTO public.products (name, description, price, stock)
VALUES
    ('Notebook Lenovo ThinkPad T14 Gen 5',  'Intel Core Ultra 7, 32 GB RAM, SSD 1 TB, pantalla 14" 2.2K',            1850.00, 12),
    ('Notebook Dell Latitude 5450',         'Intel Core Ultra 5, 16 GB RAM, SSD 512 GB, pantalla 14" FHD',            1290.00, 20),
    ('MacBook Air 13" M3',                  'Apple M3 8 nucleos, 16 GB RAM, SSD 512 GB',                              1499.00,  8),
    ('Notebook HP ProBook 450 G10',         'Intel Core i5 13va gen, 16 GB RAM, SSD 512 GB, pantalla 15.6"',           980.00, 15),
    ('PC de escritorio Dell OptiPlex 7010', 'Intel Core i7 13va gen, 32 GB RAM, SSD 1 TB, Windows 11 Pro',            1150.00, 10),
    ('Mini PC Intel NUC 13 Pro',            'Intel Core i5 1340P, 16 GB RAM, SSD 512 GB',                              720.00,  6),

    ('Monitor Dell UltraSharp U2724D',      'Monitor 27" QHD IPS, USB-C, 120 Hz',                                      489.90, 18),
    ('Monitor LG 34WP65C-B',                'Monitor curvo 34" UltraWide QHD, 160 Hz',                                 429.00,  7),

    ('Teclado Logitech MX Keys S',          'Teclado inalambrico retroiluminado, Bluetooth y receptor USB',            109.99, 40),
    ('Mouse Logitech MX Master 3S',         'Mouse inalambrico ergonomico, 8000 DPI, clics silenciosos',                99.99, 45),
    ('Webcam Logitech Brio 4K',             'Camara web 4K Ultra HD con HDR y microfono dual',                         199.00, 14),
    ('Auriculares Jabra Evolve2 65',        'Auriculares Bluetooth con cancelacion de ruido, certificados para Teams', 249.00, 25),

    ('SSD Samsung 990 PRO 2 TB',            'Unidad NVMe PCIe 4.0 M.2, hasta 7450 MB/s',                               179.99, 30),
    ('Memoria RAM Kingston Fury 32 GB',     'Kit 2x16 GB DDR5 5600 MHz',                                               119.00, 35),
    ('Disco externo WD My Passport 5 TB',   'Disco duro portatil USB 3.2 con cifrado por hardware',                    139.00, 22),
    ('Tarjeta grafica NVIDIA RTX 4070',     'GPU 12 GB GDDR6X para diseno y desarrollo de IA',                         599.00,  4),

    ('Router TP-Link Archer AX55',          'Router Wi-Fi 6 doble banda AX3000',                                        89.90, 16),
    ('Switch Ubiquiti UniFi 24 PoE',        'Switch administrable 24 puertos Gigabit PoE+',                            379.00,  5),
    ('UPS APC Back-UPS 1500VA',             'Sistema de alimentacion ininterrumpida 1500VA / 900W',                    229.00,  9),
    ('Docking Station Dell WD19S',          'Dock USB-C 180 W con salida dual de video',                               215.00,  0)
ON CONFLICT ((lower(name))) DO NOTHING;
