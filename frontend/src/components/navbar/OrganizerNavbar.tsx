import LogoImg from "../../assets/Logo.png"

export function OrganizerNavbar(){
    return(
        <div className="fixed top-0 left-0 right-0 z-50 bg-[var(--color-dark)]/95 backdrop-blur-md border-b border-[var(--color-secondary)]/30">
            <div className="max-w-7xl mx-auto px-6 h-18 flex justify-between items-center">

                {/* Logo sa prepoznatljivim sjajem */}
                <div className="flex items-center group cursor-pointer">
                    <div className="relative w-12 h-12 flex items-center justify-center">
                        <div className="absolute inset-0 bg-[var(--color-secondary)] opacity-0 group-hover:opacity-20 blur-lg transition-all duration-500 rounded-full"></div>
                        <img src={LogoImg} alt="Logo" className="relative z-10 w-full h-full object-contain" />
                    </div>
                    <div className="ml-3 text-white font-black text-base uppercase tracking-tighter">
                        Organizer<span className="text-[var(--color-secondary)]">Hub</span>
                    </div>
                </div>

                <ul className="hidden md:flex items-center gap-10 text-[10px] font-bold uppercase tracking-widest text-white/80">
                    <li><a href="#" className="hover:text-[var(--color-secondary)] transition-all">Dashboard</a></li>
                    <li><a href="#" className="hover:text-[var(--color-secondary)] transition-all">My Races</a></li>
                    <li><a href="#" className="hover:text-[var(--color-secondary)] transition-all">My Obstacles</a></li>
                </ul>

                <div className="flex items-center gap-4">
                    <div className="px-4 py-2 border border-white/10 text-[10px] font-bold uppercase tracking-widest text-white/60 hover:bg-white/5 cursor-pointer">Profile</div>
                    <div className="px-5 py-2 bg-[var(--color-secondary)] text-white text-[10px] font-black uppercase tracking-widest cursor-pointer hover:bg-[var(--color-mud)] transition-all active:scale-95">Logout</div>
                </div>
            </div>
        </div>
    )
}
export default OrganizerNavbar;