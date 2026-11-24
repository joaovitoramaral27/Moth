import json
import os
from core.moth_core import MothCore
from core.context import Context
from core.orchestrator import Orchestrator

def load_config():
    config_path = "config.json"
    if not os.path.exists(config_path):
        raise FileNotFoundError("Arquivo config.json não encontrado!")
    with open(config_path, "r", encoding="utf-8") as file:
        config = json.load(file)
    return config

def main():
    print("Inicializando Moth...")
    config = load_config()
    moth = MothCore(config)
    moth.initialize()
    context = Context(
        max_messages=config["memory"]["short_term_limit"]
    )
    orchestrator = Orchestrator(moth, context)
    print("\nTestando comunicação:")
    response = orchestrator.process_message("Olá, Moth!")
    print(f"Moth: {response}")
    print("\nHistórico da conversa:")
    print(context.get_messages())

if __name__ == "__main__":
    main()