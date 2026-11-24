class Orchestrator:

    def __init__(self, moth_core, context):
        self.moth_core = moth_core
        self.context = context

    def process_message(self, message):

        if not self.moth_core.initialized:
            raise RuntimeError("Moth não foi inicializada!")
        if not message.strip():
            return
        print("Orquestrador recebeu uma mensagem.")
        self.context.add_message("user", message)
        response = self.generate_response(message)
        self.context.add_message("assistant", response)
        return response

    def generate_response(self, message):
        return f"Mensagem recebida: {message}"