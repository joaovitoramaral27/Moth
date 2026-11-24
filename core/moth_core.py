class MothCore:

    def __init__(self, config):
        self.config = config
        self.name = config["app"]["name"]
        self.version = config["app"]["version"]
        self.initialized = False

    def initialize(self):
        print("Preparando núcleo da Moth...")
        self.initialized = True
        print(f"{self.name} v{self.version}")
        print("Núcleo inicializado com sucesso!")