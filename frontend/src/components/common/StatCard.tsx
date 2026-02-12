export const StatCard = ({ label, value, color = "text-white", opacity = "" }: any) => (
    <div className={`bg-white/3 border border-white/10 p-8 ${opacity}`}>
        <span className="text-[9px] uppercase tracking-widest text-white/20 block mb-2">{label}</span>
        <span className={`text-4xl ${color} italic leading-none`}>{value || 0}</span>
    </div>
);