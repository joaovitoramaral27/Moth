class Context:

    def __init__(self, max_messages=10):
        self.max_messages = max_messages
        self.messages = []

    def add_message(self, role, content):

        if role not in ("user", "assistant", "system"):
            raise ValueError("Tipo de mensagem inválido!")
        if not content.strip():
            return
        self.messages.append({
            "role": role,
            "content": content
        })
        if len(self.messages) > self.max_messages:
            self.messages.pop(0)

    def get_messages(self):
        return self.messages.copy()

    def clear(self):
        self.messages.clear()