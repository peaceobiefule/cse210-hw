import time
import random
import sys
import os

# ANSI color codes
class Colors:
    HEADER = "\033[95m"
    OKBLUE = "\033[94m"
    OKCYAN = "\033[96m"
    OKGREEN = "\033[92m"
    OKYELLOW = "\033[93m"
    OKRED = "\033[91m"
    BOLD = "\033[1m"
    UNDERLINE = "\033[4m"
    END = "\033[0m"

# ------------------ Base Class ------------------
class MindfulnessActivity:
    def __init__(self, name, description):
        self._name = name
        self._description = description
        self._duration = 0

    def start_message(self):
        os.system('cls' if os.name == 'nt' else 'clear')
        print(f"\n{Colors.BOLD}{Colors.OKBLUE}--- {self._name} ---{Colors.END}")
        print(f"{Colors.OKCYAN}{self._description}{Colors.END}")
        while True:
            try:
                self._duration = int(input(f"{Colors.OKYELLOW}Enter duration in seconds: {Colors.END}"))
                break
            except ValueError:
                print(f"{Colors.OKRED}Please enter a valid number.{Colors.END}")
        print(f"{Colors.BOLD}Prepare to begin...{Colors.END}")
        self._spinner_pause(3)

    def end_message(self):
        print(f"\n{Colors.OKGREEN}Well done!{Colors.END}")
        self._spinner_pause(2)
        print(f"{Colors.OKCYAN}You have completed the {self._name} for {self._duration} seconds.{Colors.END}")
        self._spinner_pause(3)

    def _spinner_pause(self, seconds):
        spinner_symbols = [f"{Colors.OKBLUE}|{Colors.END}",
                           f"{Colors.OKGREEN}/{Colors.END}",
                           f"{Colors.OKCYAN}-{Colors.END}",
                           f"{Colors.OKYELLOW}\\{Colors.END}"]
        for _ in range(seconds * 4):
            for symbol in spinner_symbols:
                sys.stdout.write(f"\r{symbol} ")
                sys.stdout.flush()
                time.sleep(0.25)
        print("\r", end="")

    def _countdown(self, seconds):
        for i in range(seconds, 0, -1):
            sys.stdout.write(f"\r{Colors.BOLD}{Colors.OKYELLOW}{i}...{Colors.END}")
            sys.stdout.flush()
            time.sleep(1)
        print("\r", end="")

# ------------------ Breathing Activity ------------------
class BreathingActivity(MindfulnessActivity):
    def __init__(self):
        description = (
            "This activity will help you relax by walking you through breathing in and out slowly. "
            "Clear your mind and focus on your breathing."
        )
        super().__init__("Breathing Activity", description)

    def _breathing_animation(self, expanding=True, duration=3):
        frames = [
            "   ( )   ",
            "  (   )  ",
            " (     ) ",
            "(       )",
            " (     ) ",
            "  (   )  ",
            "   ( )   "
        ]
        if not expanding:
            frames = list(reversed(frames))

        step_time = duration / len(frames)
        for frame in frames:
            sys.stdout.write(f"\r{Colors.OKCYAN}{frame}{Colors.END}")
            sys.stdout.flush()
            time.sleep(step_time)
        print("\r", end="")

    def run(self):
        self.start_message()
        elapsed = 0
        while elapsed < self._duration:
            if elapsed + 3 > self._duration:
                break
            print(f"\n{Colors.OKGREEN}Breathe in...{Colors.END}")
            self._breathing_animation(expanding=True, duration=3)
            elapsed += 3
            if elapsed + 3 > self._duration:
                break
            print(f"\n{Colors.OKBLUE}Breathe out...{Colors.END}")
            self._breathing_animation(expanding=False, duration=3)
            elapsed += 3
        self.end_message()

# ------------------ Reflection Activity ------------------
class ReflectionActivity(MindfulnessActivity):
    def __init__(self):
        description = (
            "This activity will help you reflect on times in your life when you have shown strength and resilience. "
            "This will help you recognize the power you have and how you can use it in other aspects of your life."
        )
        super().__init__("Reflection Activity", description)
        self._prompts = [
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        ]
        self._questions = [
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        ]

    def run(self):
        self.start_message()
        print(f"{Colors.OKYELLOW}{random.choice(self._prompts)}{Colors.END}")
        elapsed = 0
        used_questions = random.sample(self._questions, len(self._questions))
        q_index = 0
        while elapsed < self._duration:
            if q_index >= len(used_questions):
                q_index = 0
            print(f"{Colors.OKCYAN}{used_questions[q_index]}{Colors.END}")
            self._spinner_pause(4)
            elapsed += 4
            q_index += 1
        self.end_message()

# ------------------ Listing Activity ------------------
class ListingActivity(MindfulnessActivity):
    def __init__(self):
        description = (
            "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area."
        )
        super().__init__("Listing Activity", description)
        self._prompts = [
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        ]

    def run(self):
        self.start_message()
        prompt = random.choice(self._prompts)
        print(f"{Colors.OKYELLOW}{prompt}{Colors.END}")
        print(f"{Colors.OKGREEN}You will begin listing in...{Colors.END}")
        self._countdown(3)
        print(f"{Colors.BOLD}Start listing! (Press Enter after each item){Colors.END}")
        
        start_time = time.time()
        items = []
        while time.time() - start_time < self._duration:
            item = input(f"{Colors.OKBLUE}> {Colors.END}")
            items.append(item)
        print(f"\n{Colors.OKCYAN}You listed {len(items)} items.{Colors.END}")
        self.end_message()

# ------------------ Menu ------------------
def main():
    while True:
        print(f"\n{Colors.BOLD}{Colors.HEADER}Mindfulness Program{Colors.END}")
        print(f"{Colors.OKGREEN}1.{Colors.END} Breathing Activity")
        print(f"{Colors.OKGREEN}2.{Colors.END} Reflection Activity")
        print(f"{Colors.OKGREEN}3.{Colors.END} Listing Activity")
        print(f"{Colors.OKGREEN}4.{Colors.END} Quit")
        choice = input(f"{Colors.OKYELLOW}Choose an option: {Colors.END}")
        if choice == "1":
            BreathingActivity().run()
        elif choice == "2":
            ReflectionActivity().run()
        elif choice == "3":
            ListingActivity().run()
        elif choice == "4":
            print(f"{Colors.OKRED}Goodbye! Stay mindful.{Colors.END}")
            break
        else:
            print(f"{Colors.OKRED}Invalid choice. Try again.{Colors.END}")

if __name__ == "__main__":
    main()
